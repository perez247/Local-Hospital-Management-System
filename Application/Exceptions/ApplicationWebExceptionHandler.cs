using Application.RequestResponsePipeline;
using Application.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApplicationWebExceptionHandler : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _env;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="ienv"></param>
        public ApplicationWebExceptionHandler(IHostingEnvironment ienv)
        {
            _env = ienv;
        }

        /// <summary>
        /// Override default exception event action
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            //throw new Exception("I do not understand");
            //var jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

            if (context.Exception is AppFluentValidationException)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(
                    new ApplicationErrorResponse()
                    {
                        Errors = ((AppFluentValidationException)context.Exception).Failures,
                        Environment = EnvironmentFunctions.ASPNETCORE_ENVIRONMENT
                    }
                );

                return;
            }

            var code = (int)HttpStatusCode.InternalServerError;
            var exception = context.Exception as IGeneralException;

            if (exception != null)
            {
                code = (int)exception.StatusCode;
            }

            var errorresponse = new ApplicationErrorResponse() { Error = context.Exception.Message, Environment = _env.EnvironmentName.ToLower() };

            //errorresponse.StackTrace = !EnvironmentFunctions.IsProduction() ? context.Exception.StackTrace : null;
            errorresponse.StackTrace = true ? context.Exception.StackTrace : null;

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = 500;
            context.Result = new JsonResult(errorresponse);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomValidationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                //var jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

                var appErrors = new List<CustomValidationFailure>();

                foreach (var item in context.ModelState)
                {
                    appErrors.Add(new CustomValidationFailure
                    {
                        FieldName = item.Key,
                        FieldErrors = item.Value.Errors.Select(x => x.ErrorMessage).ToArray()
                    });
                }

                var responseObj = new ApplicationErrorResponse
                {
                    Environment = EnvironmentFunctions.ASPNETCORE_ENVIRONMENT,
                    Errors = appErrors
                };

                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(responseObj);
            }
        }
    }
}

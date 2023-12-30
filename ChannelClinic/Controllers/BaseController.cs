using Application.Interfaces.IRepositories;
using Application.RequestResponsePipeline;
using Application.Utilities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChannelClinic.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Contains all the 
        /// </summary>
        public ApplicationUserRequest? ApplicationUserRequest { get; set; }

        /// <summary>
        /// Set initial requirements
        /// </summary>
        public BaseController(IMediator mediator, IUserRepository userRepository)
        {
            ApplicationUserRequest = new ApplicationUserRequest
            {
                User = User,
                Mediator = mediator,
                HttpContext = HttpContext,
                UserRepository= userRepository
            };
        }

        /// <summary>
        /// Set the token to be used in the application
        /// </summary>
        /// <param name="token"></param>
        /// <param name="requestName"></param>
        protected async Task UpdateToken(TokenCredentials token, string requestName)
        {
            string userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUserRequest.CurrentUser = await ApplicationUserRequest.UserRepository?.GetUserByIdOrEmailAsync(userId, "");
            ApplicationUserRequest.RequestName = requestName ?? "Unknown";
            token.SetCurrentUserRequest(ApplicationUserRequest);
        }

    }
}

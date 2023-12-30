using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class CustomMessageException : Exception, IGeneralException
    {
        /// <summary>
        /// Gets the email address
        /// </summary>
        /// <param name="errorMessage">Message to be given back to the client or Admin's email address</param>
        /// <param name="statusCode">Status code of the error</param>
        public CustomMessageException(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            : base(errorMessage)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Status code to use so the application will understand
        /// </summary>
        /// <value></value>
        public HttpStatusCode StatusCode { get; set; }
    }
}

using Application.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RequestResponsePipeline
{
    public class ApplicationErrorResponse
    {
        /// <summary>
        /// This is basically used for for sending validation errors
        /// This contains multiple errors and will be stored as a list of error
        /// </summary>
        /// <value>list{objects}</value>
        [JsonProperty("errors")]
        public ICollection<CustomValidationFailure>? Errors { get; set; }

        /// <summary>
        /// This should contain only a single error 
        /// This is any other error
        /// </summary>
        /// <value>string</value>
        [JsonProperty("error")]
        public string? Error { get; set; }

        /// <summary>
        /// Determines what environment this is
        /// </summary>
        /// <value>string</value>
        [JsonProperty("environment")]
        public string? Environment { get; set; }

        /// <summary>
        /// This is used for debugging and should show only in development or staging environment
        /// </summary>
        /// <value></value>
        [JsonProperty("stackTrace")]
        public string? StackTrace { get; set; }
    }
}

using Application.RequestResponsePipeline;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities
{
    public class TokenCredentials
    {
        private ApplicationUserRequest? ApplicationUserRequest { get; set; }

        public void SetCurrentUserRequest(ApplicationUserRequest? applicationUserRequest)
        {
            ApplicationUserRequest = applicationUserRequest;
        }

        public ApplicationUserRequest getCurrentUserRequest()
        {
            return ApplicationUserRequest;
        }
    }
}

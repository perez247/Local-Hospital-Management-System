using Application.Command.AuthEntities.SignIn;
using Application.Interfaces.IRepositories;
using Application.Query.AuthEntities.GetLookUps;
using Application.Query.UserEntities.GetUsersInDev;
using Application.RequestResponsePipeline;
using Application.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChannelClinic.Controllers
{
    /// <summary>
    /// Auth controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AuthController(IMediator mediator, IUserRepository userRepository) 
            : base(mediator, userRepository) { }

        /// <summary>
        /// Sign in to the application
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(SignInResponse), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsersForDev(GetUsersInDevQuery command)
        {
            if (EnvironmentFunctions.isEnv("Production"))
            {
                return BadRequest();
            }

            await UpdateToken(command, nameof(GetUsersInDevQuery));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Sign in to the application
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(SignInResponse), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> ValidateOtp([FromBody] SignInCommand command)
        {
            //command.SetCurrentUserRequest(ApplicationUserRequest);
            await UpdateToken(command, nameof(SignInCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Get app lookups
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<LookUpResponse>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        [HttpGet("lookups")]
        public async Task<IActionResult> GetLookups([FromQuery] GetLookUpsQuery command)
        {
            //command.SetCurrentUserRequest(ApplicationUserRequest);
            await UpdateToken(command, nameof(GetLookUpsQuery));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }
    }
}

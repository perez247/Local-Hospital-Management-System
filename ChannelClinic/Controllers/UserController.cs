using Application.Command.CreateStaff;
using Application.Command.UpdateSurgerySummary;
using Application.Command.UpdateUserNextofKin;
using Application.Command.UpdateUserPersonal;
using Application.Interfaces.IRepositories;
using Application.RequestResponsePipeline;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChannelClinic.Controllers
{
    /// <summary>
    /// User controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UserController(IMediator mediator, IUserRepository userRepository)
            : base(mediator, userRepository) { }

        /// <summary>
        /// Update user personal
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("personal")]
        public async Task<IActionResult> UpdatePersonalDetails([FromBody] UpdateUserPersonalCommand command)
        {
            await UpdateToken(command, nameof(UpdateUserPersonalCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Update user next of kin
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("next-of-kin")]
        public async Task<IActionResult> UpdateNextOfKin([FromBody] UpdateUserNextofKinCommand command)
        {
            await UpdateToken(command, nameof(UpdateUserNextofKinCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Personnel involved in a surgery to give summary
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("surgery-summary")]
        public async Task<IActionResult> GiveSummeryOnSurgery([FromBody] UpdateSurgerySummaryCommand command)
        {
            await UpdateToken(command, nameof(UpdateSurgerySummaryCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }
    }
}

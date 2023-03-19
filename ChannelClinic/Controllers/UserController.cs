using Application.Command.UpdateSurgerySummary;
using Application.Command.UserEntities.AddUserFiles;
using Application.Command.UserEntities.DeleteUserFiles;
using Application.Command.UserEntities.UpdateUserNextofKin;
using Application.Command.UserEntities.UpdateUserPersonal;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.UserEntities.GetUserFiles;
using Application.Query.UserEntities.GetUserList;
using Application.RequestResponsePipeline;
using Application.Responses;
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

        /// <summary>
        /// Get a user or list of users
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PaginationResponse<IEnumerable<UserResponse>>), (int)HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IActionResult> GetUserList([FromBody] GetUserListQuery command)
        {
            await UpdateToken(command, nameof(GetUserListQuery));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Add a user files
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("files")]
        public async Task<IActionResult> AddUserFiles([FromBody] AddUserFilesCommand command)
        {
            await UpdateToken(command, nameof(AddUserFilesCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Delete user files
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpDelete("files")]
        public async Task<IActionResult> DeleteUserFiles([FromBody] DeleteUserFIlesCommand command)
        {
            await UpdateToken(command, nameof(DeleteUserFIlesCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }


        /// <summary>
        /// Get user files
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<UserFileResponse>), (int)HttpStatusCode.OK)]
        [HttpGet("files")]
        public async Task<IActionResult> GetUserFiles([FromQuery] GetUserFIlesQuery command)
        {
            await UpdateToken(command, nameof(GetUserFIlesQuery));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }
    }
}

using Application.Command.AddStaffTimeTable;
using Application.Command.UpdateStaffShift;
using Application.Interfaces.IRepositories;
using Application.RequestResponsePipeline;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChannelClinic.Controllers
{

    /// <summary>
    /// Time table controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TimeTableController : BaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TimeTableController(IMediator mediator, IUserRepository userRepository)
            : base(mediator, userRepository) { }

        /// <summary>
        /// Add a list of staff to a shift
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("add-to-shift")]
        public async Task<IActionResult> AddStaffToShift([FromBody] AddStaffTimeTableCommand command)
        {
            await UpdateToken(command, nameof(AddStaffTimeTableCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// update staff shifts
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("update-shifts")]
        public async Task<IActionResult> UpdatetaffShift([FromBody] UpdateStaffShiftCommand command)
        {
            await UpdateToken(command, nameof(UpdateStaffShiftCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

    }
}

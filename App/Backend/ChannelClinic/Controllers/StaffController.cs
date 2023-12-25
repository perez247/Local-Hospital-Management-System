using Application.Command.StaffEntities.CreateStaff;
using Application.Command.StaffEntities.UpdateStaffDetails;
using Application.Command.StaffEntities.UpdateStaffRoles;
using Application.Interfaces.IRepositories;
using Application.Query.StaffEntities.GetDashboardStats;
using Application.RequestResponsePipeline;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChannelClinic.Controllers
{

    /// <summary>
    /// Staff controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : BaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StaffController(IMediator mediator, IUserRepository userRepository)
            : base(mediator, userRepository) { }

        /// <summary>
        /// Create a new staff
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CreateStaffResponse), (int)HttpStatusCode.OK)]
        [HttpPost("create")]
        public async Task<IActionResult> CreateStaff([FromBody] CreateStaffCommand command)
        {
            await UpdateToken(command, nameof(CreateStaffCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Update a staff details
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("staff")]
        public async Task<IActionResult> UpdateStaff([FromBody] UpdateStaffDetailCommand command)
        {
            await UpdateToken(command, nameof(UpdateStaffDetailCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Update staff Roles
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("update-roles")]
        public async Task<IActionResult> UpdateStaffRoles([FromBody] UpdateStaffRolesCommand command)
        {
            await UpdateToken(command, nameof(UpdateStaffRolesCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Get dashboard stats
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(DashboardStatsResponse), (int)HttpStatusCode.OK)]
        [HttpGet("dashboard-stats")]
        public async Task<IActionResult> DashboardStats([FromQuery] GetDashboardStatsQuery command)
        {
            await UpdateToken(command, nameof(GetDashboardStatsQuery));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }
    }
}

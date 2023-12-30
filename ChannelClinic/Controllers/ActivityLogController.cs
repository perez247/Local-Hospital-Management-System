using Application.Command.InventoryEntities.SaveInventoryItem;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.ActivityLogEntities.GetActivityLog;
using Application.RequestResponsePipeline;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChannelClinic.Controllers
{
    /// <summary>
    /// Activity Log controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityLogController : BaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ActivityLogController(IMediator mediator, IUserRepository userRepository)
            : base(mediator, userRepository) { }

        /// <summary>
        /// Get logs
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PaginationResponse<IEnumerable<ActivityLogResponse>>), (int)HttpStatusCode.OK)]
        [HttpPost("get-logs")]
        public async Task<IActionResult> GetLogs([FromBody] GetActivityLogQuery command)
        {
            await UpdateToken(command, nameof(GetActivityLogQuery));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }
    }
}

using Application.Interfaces.IRepositories;
using Application.Query.TicketEntities.GetAdmissionStats;
using Application.RequestResponsePipeline;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChannelClinic.Controllers
{
    /// <summary>
    /// Admission controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AdmissionController : BaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AdmissionController(IMediator mediator, IUserRepository userRepository)
            : base(mediator, userRepository) { }


        /// <summary>
        /// Update single Admission stats
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(GetAdmissionStatsResponse), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<IActionResult> UpdateSingleTicket([FromQuery] GetAdmissionStatsQuery command)
        {
            await UpdateToken(command, nameof(GetAdmissionStatsQuery));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

    }
}

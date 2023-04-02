using Application.Command.TicketEntities.AddPharmacyTicketInventory;
using Application.Command.TicketEntities.ConcludePharmacyTicket;
using Application.Command.TicketEntities.DeleteTicket;
using Application.Command.TicketEntities.SendAllTicketsToDepartment;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.TicketEntities.GetTickets;
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
    public class TicketController : BaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TicketController(IMediator mediator, IUserRepository userRepository)
            : base(mediator, userRepository) { }


        /// <summary>
        /// Add pharmacy ticket inventory to the paharmacy ticket by a doctor
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("pharmacy-ticket-inventory")]
        public async Task<IActionResult> SavePharmacyTicketInventory([FromBody] AddPharmacyTicketInventoryCommand command)
        {
            await UpdateToken(command, nameof(AddPharmacyTicketInventoryCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// conclude a list of pharmacy ticket inventory of an appointment
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("conclude-pharmacy-ticket-inventory")]
        public async Task<IActionResult> ConcludePharmacyTicketInventory([FromBody] ConcludePharmacyTicketCommand command)
        {
            await UpdateToken(command, nameof(ConcludePharmacyTicketCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Get Tickets
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PaginationResponse<IEnumerable<AppTicketResponse>>), (int)HttpStatusCode.OK)]
        [HttpPost("get-tickets")]
        public async Task<IActionResult> GetTickets([FromBody] GetTicketsQuery command)
        {
            await UpdateToken(command, nameof(GetTicketsQuery));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Send all tickets to the departments
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("send-to-departments")]
        public async Task<IActionResult> SendAllTicketsToDepartment([FromBody] SendAllTicketsToDepartmentCommand command)
        {
            await UpdateToken(command, nameof(SendAllTicketsToDepartmentCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }


        /// <summary>
        /// Remove app ticket
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpDelete("app-ticket")]
        public async Task<IActionResult> DeleteTicket([FromBody] DeleteTicketCommand command)
        {
            await UpdateToken(command, nameof(DeleteTicketCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }
    }
}

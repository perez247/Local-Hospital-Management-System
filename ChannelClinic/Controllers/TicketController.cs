using Application.Command.TicketEntities.AddPharmacyTicketInventory;
using Application.Command.TicketEntities.ConcludeLabRadTicket;
using Application.Command.TicketEntities.ConcludePharmacyTicket;
using Application.Command.TicketEntities.DeleteTicket;
using Application.Command.TicketEntities.SaveTicketAndInventory;
using Application.Command.TicketEntities.SendAllTicketsToDepartment;
using Application.Command.TicketEntities.SendLabTicketsToFinance;
using Application.Command.TicketEntities.SendPharmacyTicketToFinance;
using Application.Command.TicketEntities.UpdateTicket;
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
        /// Update single ticket
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut]
        public async Task<IActionResult> UpdateSingleTicket([FromBody] UpdateTicketCommand command)
        {
            await UpdateToken(command, nameof(UpdateTicketCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Add pharmacy ticket inventory to the paharmacy ticket by a doctor
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("pharmacy-ticket-inventory")]
        public async Task<IActionResult> SavePharmacyTicketInventory([FromBody] SaveTicketAndInventoryCommand command)
        {
            await UpdateToken(command, nameof(SaveTicketAndInventoryCommand));
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
        /// conclude a list of Lab or Radiology ticket inventory of an appointment
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("conclude-lab-ticket-inventory")]
        public async Task<IActionResult> ConcludeLabRadTicketInventory([FromBody] ConcludeLabRadTicketCommand command)
        {
            await UpdateToken(command, nameof(ConcludeLabRadTicketCommand));
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

        /// <summary>
        /// Send pharmacy tickets to finance
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("send-pharmacy-to-finance")]
        public async Task<IActionResult> SendPharmacyToFinance([FromBody] SendPharmacyTicketToFinanceCommand command)
        {
            await UpdateToken(command, nameof(SendPharmacyTicketToFinanceCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Send lab tickets to finance
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("send-lab-to-finance")]
        public async Task<IActionResult> SendLabToFinance([FromBody] SendLabTicketsToFinanceCommand command)
        {
            await UpdateToken(command, nameof(SendLabTicketsToFinanceCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }
    }
}

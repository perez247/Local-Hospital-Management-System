using Application.Command.AddAdmissionTicketInventory;
using Application.Command.AddAppointment;
using Application.Command.AddLabTicketInventory;
using Application.Command.AddPharmacyTicketInventory;
using Application.Command.ConcludeAdmissionTicketInventory;
using Application.Command.ConcludeLabTicketInventory;
using Application.Command.ConcludePharmacyTicket;
using Application.Command.ConcludeSurgeryTicketInventory;
using Application.Command.CreateEmergencyAppoitment;
using Application.Command.CreateTicket;
using Application.Command.SaveSurgeryTicketInventory;
using Application.Command.UpdateLabTicketInventory;
using Application.Command.UpdatePharmacyTicketInventory;
using Application.Command.UpdateSurgeryTicketInventory;
using Application.Command.UpdateTicket;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.GetTickets;
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
        /// Add a ticket to an appointment by a doctor
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IActionResult> AddTicket([FromBody] CreateTicketCommand command)
        {
            await UpdateToken(command, nameof(CreateTicketCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// update ticket of an appointment
        /// Doctors can only send to department (only once) and update ovreal description
        /// Staff in department can only send to finance for a decision only once
        /// Once sent, you cannot send again
        /// Once sent to finance, you cannot send to finance again
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut]
        public async Task<IActionResult> UpateTicket([FromBody] UpdateTicketCommand command)
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
        public async Task<IActionResult> SavePharmacyTicketInventory([FromBody] AddPharmacyTicketInventoryCommand command)
        {
            await UpdateToken(command, nameof(AddPharmacyTicketInventoryCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// update a list of pharmacy ticket inventory of an appointment by a staff
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("pharmacy-ticket-inventory")]
        public async Task<IActionResult> UpateTicketInventory([FromBody] UpdatePharmacyTicketInventoryCommand command)
        {
            await UpdateToken(command, nameof(UpdatePharmacyTicketInventoryCommand));
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
        /// Add a list of surgery ticket inventory to the paharmacy ticket by a doctor
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("surgery-ticket-inventory")]
        public async Task<IActionResult> SaveSurgeryTicketInventory([FromBody] SaveSurgeryTicketInventoryCommand command)
        {
            await UpdateToken(command, nameof(SaveSurgeryTicketInventoryCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// upadate a list of surgery ticket inventory to the paharmacy ticket by a staff
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("surgery-ticket-inventory")]
        public async Task<IActionResult> UpdateSurgeryTicketInventory([FromBody] UpdateSurgeryTicketInventoryCommand command)
        {
            await UpdateToken(command, nameof(UpdateSurgeryTicketInventoryCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// conclude a list of surgery ticket inventory of an appointment
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("conclude-surgery-ticket-inventory")]
        public async Task<IActionResult> ConcludeSurgeryTicketInventory([FromBody] ConcludeSurgeryTicketInventoryCommand command)
        {
            await UpdateToken(command, nameof(ConcludeSurgeryTicketInventoryCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Add a list of lab ticket inventory to the paharmacy ticket by a doctor
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("lab-ticket-inventory")]
        public async Task<IActionResult> SaveLabTicketInventory([FromBody] AddLabTicketInventoryCommand command)
        {
            await UpdateToken(command, nameof(AddLabTicketInventoryCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Update a list of lab ticket inventory to the paharmacy ticket by a doctor
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("lab-ticket-inventory")]
        public async Task<IActionResult> UpdateLabTicketInventory([FromBody] UpdateLabTicketInventoryCommand command)
        {
            await UpdateToken(command, nameof(UpdateLabTicketInventoryCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// conclude a list of lab ticket inventory of an appointment
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("conclude-lab-ticket-inventory")]
        public async Task<IActionResult> ConcludeLabTicketInventory([FromBody] ConcludeLabTicketInventoryCommand command)
        {
            await UpdateToken(command, nameof(ConcludeLabTicketInventoryCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Add a admission ticket by a doctor
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("admission-ticket-inventory")]
        public async Task<IActionResult> SaveAdmissionTicketInventory([FromBody] AddAdmissionTicketInventoryCommand command)
        {
            await UpdateToken(command, nameof(AddAdmissionTicketInventoryCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// conclude an admission ticket inventory of an appointment
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("conclude-andmission-ticket-inventory")]
        public async Task<IActionResult> ConcludeAdmissionTicketInventory([FromBody] ConcludeAdmissionTicketInventoryCommand command)
        {
            await UpdateToken(command, nameof(ConcludeAdmissionTicketInventoryCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Create an emergency ticket quickly
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("emergency-appointment")]
        public async Task<IActionResult> CreateEmergencyAppointment([FromBody] CreateEmergencyAppointmentCommand command)
        {
            await UpdateToken(command, nameof(CreateEmergencyAppointmentCommand));
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
    }
}

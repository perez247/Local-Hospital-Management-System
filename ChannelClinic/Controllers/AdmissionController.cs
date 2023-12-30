using Application.Command.AdmissionEntities.DeleteAdmissionPrescription;
using Application.Command.AdmissionEntities.ExecutePrescription;
using Application.Command.AdmissionEntities.SaveAdmissionPrescription;
using Application.Command.TicketEntities.SaveTicketAndInventory;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.AdmissionEntities.GetPrescriptions;
using Application.Query.TicketEntities.GetAdmissionStats;
using Application.RequestResponsePipeline;
using Application.Responses;
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

        /// <summary>
        /// Create admission prescription
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("prescription")]
        public async Task<IActionResult> CreateAdmissionPrescription([FromBody] SaveAdmissionPrescriptionCommand command)
        {
            await UpdateToken(command, nameof(SaveAdmissionPrescriptionCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// get admission prescriptions
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PaginationResponse<IEnumerable<AdmissionPrescriptionResponse>>), (int)HttpStatusCode.OK)]
        [HttpPost("get-prescriptions")]
        public async Task<IActionResult> GetAdmissionPrescription([FromBody] GetPrescriptionsQuery command)
        {
            await UpdateToken(command, nameof(GetPrescriptionsQuery));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Delete a prescription that has not been concluded
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpDelete("prescription")]
        public async Task<IActionResult> DeletePrescription([FromBody] DeleteAdmissionPrescriptionCommand command)
        {
            await UpdateToken(command, nameof(DeleteAdmissionPrescriptionCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Execute the prescription given by the doctor
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("execute-prescription")]
        public async Task<IActionResult> ExecutePrescription([FromBody] ExecutePrescriptionCommand command)
        {
            await UpdateToken(command, nameof(ExecutePrescriptionCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }
    }
}

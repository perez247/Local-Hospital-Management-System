using Application.Command.AppointmentEntities.AddAppointment;
using Application.Command.AppointmentEntities.UpdateAppointment;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.AppointmentEntities.GetAppointmentCountInAMonth;
using Application.Query.AppointmentEntities.GetAppointments;
using Application.RequestResponsePipeline;
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
    public class AppointmentController : BaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AppointmentController(IMediator mediator, IUserRepository userRepository)
            : base(mediator, userRepository) { }

        /// <summary>
        /// Add an appointment
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(AddAppointmentResponse), (int)HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IActionResult> AddAppointment([FromBody] AppAppointmentCommand command)
        {
            await UpdateToken(command, nameof(AppAppointmentCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }


        /// <summary>
        /// update an appointment
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut]
        public async Task<IActionResult> UpdateAppointment([FromBody] UpdateAppointmentCommand command)
        {
            await UpdateToken(command, nameof(UpdateAppointmentCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Get list of appointments by date
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PaginationResponse<IEnumerable<AppointmentResponse>>), (int)HttpStatusCode.OK)]
        [HttpPost("appointment-by-date")]
        public async Task<IActionResult> GetAppointmentsByDate([FromBody] GetAppointmentQuery command)
        {
            await UpdateToken(command, nameof(GetAppointmentQuery));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }


        /// <summary>
        /// Get appointment counts in a month
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<AppointmentDayCountResponse>), (int)HttpStatusCode.OK)]
        [HttpPost("appointment-count-in-mmonth")]
        public async Task<IActionResult> GetAppointmentsCountForMonth([FromBody] GetAppointmentCountInAMonthQuery command)
        {
            await UpdateToken(command, nameof(GetAppointmentCountInAMonthQuery));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

    }
}

using Application.Command.AddAppointment;
using Application.Command.AddStaffTimeTable;
using Application.Command.CompanyPayBill;
using Application.Command.CreateMonthPayment;
using Application.Command.CreateStaff;
using Application.Command.UpdateAppCost;
using Application.Command.UpdateFinancialRecord;
using Application.Command.UpdatePaymentForMonth;
using Application.Command.UpdateStaffDetails;
using Application.Command.UpdateStaffShift;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.StaffPaymentHistory;
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
        /// Add payment for a company
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("company-payment")]
        public async Task<IActionResult> CompanyMakesPayment([FromBody] CompanyPayBillCommand command)
        {
            await UpdateToken(command, nameof(CompanyPayBillCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }


        /// <summary>
        /// Update a financial record
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("financial-record")]
        public async Task<IActionResult> UpdateFinancialRecord([FromBody] UpdateFinancialRecordCommand command)
        {
            await UpdateToken(command, nameof(UpdateFinancialRecordCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }
    }
}

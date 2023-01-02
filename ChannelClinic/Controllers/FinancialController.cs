using Application.Command.ConcludePharmacyTicket;
using Application.Command.CreateFinancialRequest;
using Application.Command.CreateMonthPayment;
using Application.Command.RespondToFinancialRequest;
using Application.Command.UpdateAppCost;
using Application.Command.UpdatePaymentForMonth;
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
    /// Financial controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FinancialController : BaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FinancialController(IMediator mediator, IUserRepository userRepository)
            : base(mediator, userRepository) { }


        /// <summary>
        /// update cost used
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("update-cost")]
        public async Task<IActionResult> UpdateCost([FromBody] UpdateAppCostCommand command)
        {
            await UpdateToken(command, nameof(UpdateAppCostCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// update cost price used based on negiotiation
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("update-cost-price")]
        public async Task<IActionResult> UpdateCostPrice([FromBody] ConcludePharmacyTicketCommand command)
        {
            await UpdateToken(command, nameof(ConcludePharmacyTicketCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }


        /// <summary>
        /// Get staff payment history
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(PaginationResponse<IEnumerable<SalaryPaymentHistoryResponse>>), (int)HttpStatusCode.OK)]
        [HttpPost("payment-history")]
        public async Task<IActionResult> GetPaymentHistory([FromBody] StaffPaymentHistoryQuery command)
        {
            await UpdateToken(command, nameof(StaffPaymentHistoryQuery));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Get staff payment history
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("create-payment-history")]
        public async Task<IActionResult> AddMonthForSalary([FromBody] CreateMonthPaymentCommand command)
        {
            await UpdateToken(command, nameof(CreateMonthPaymentCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Update staff salary for the month if paid or not
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("update-salary-payments")]
        public async Task<IActionResult> UpdateSalaryForMonth([FromBody] UpdatePaymentForMonthCommand command)
        {
            await UpdateToken(command, nameof(UpdatePaymentForMonthCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }


        /// <summary>
        /// Create a financial request for approval for either profit or expense
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("financial-request")]
        public async Task<IActionResult> CreateFinancialRequest([FromBody] CreateFinancialRequestCommand command)
        {
            await UpdateToken(command, nameof(CreateFinancialRequestCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Respond to financial request for either approval or denial
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("financial-request")]
        public async Task<IActionResult> RespondFinancialRequest([FromBody] RespondToFinancialRequestCommand command)
        {
            await UpdateToken(command, nameof(RespondToFinancialRequestCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

    }
}

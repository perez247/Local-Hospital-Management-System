using Application.Command.CreateFinancialRequest;
using Application.Command.CreateMonthPayment;
using Application.Command.FinancialRecordEntities.InitialPayment;
using Application.Command.FinancialRecordEntities.PatientUpdatePayment;
using Application.Command.FinancialRecordEntities.UpdateContract;
using Application.Command.RespondToFinancialRequest;
using Application.Command.UpdateAppCost;
using Application.Command.UpdatePaymentForMonth;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.FinancialRecordEntities.GetAppCosts;
using Application.Query.FinancialRecordEntities.GetFinancialRecords;
using Application.Query.FinancialRecordEntities.GetPendingUserContracts;
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

        /// <summary>
        /// Start initial payment
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("initial-payment")]
        public async Task<IActionResult> InitialPayment([FromBody] InitialPaymentCommand command)
        {
            await UpdateToken(command, nameof(InitialPaymentCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Update payment
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("update-patient-payment")]
        public async Task<IActionResult> UpdatePatientPayment([FromBody] PatientUpdatePaymentCommand command)
        {
            await UpdateToken(command, nameof(PatientUpdatePaymentCommand));

            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Get contracts
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PaginationResponse<IEnumerable<GetPendingUserContractsResponse>>), (int)HttpStatusCode.OK)]
        [HttpPut("get-contracts")]
        public async Task<IActionResult> GetContracts([FromBody] GetPendingUserContractsQuery command)
        {
            await UpdateToken(command, nameof(GetPendingUserContractsQuery));

            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Get contracts
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("update-contract")]
        public async Task<IActionResult> UpdateContracts([FromBody] UpdateContractCommand command)
        {
            await UpdateToken(command, nameof(UpdateContractCommand));

            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Get financial debts
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PaginationResponse<IEnumerable<AppCostResponse>>), (int)HttpStatusCode.OK)]
        [HttpPost("debts")]
        public async Task<IActionResult> FinancialDebts([FromBody] GetAppCostsQuery command)
        {
            await UpdateToken(command, nameof(GetAppCostsQuery));

            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }


        /// <summary>
        /// Get financial payments completed
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PaginationResponse<IEnumerable<FinancialRecordResponse>>), (int)HttpStatusCode.OK)]
        [HttpPost("paid")]
        public async Task<IActionResult> FinancialPaid([FromBody] GetFinancialRecordsQuery command)
        {
            await UpdateToken(command, nameof(GetFinancialRecordsQuery));

            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }
    }
}


using Application.Command.FinancialRecordEntities.BillClient;
using Application.Command.FinancialRecordEntities.InitialPayment;
using Application.Command.FinancialRecordEntities.PatientUpdatePayment;
using Application.Command.FinancialRecordEntities.PayDebt;
using Application.Command.FinancialRecordEntities.UpdateContract;
using Application.DTOs;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.FinancialRecordEntities.GetAppCosts;
using Application.Query.FinancialRecordEntities.GetFinancialRecords;
using Application.Query.FinancialRecordEntities.GetPendingUserContracts;
using Application.Query.InventoryEntities.GetTicketInventoriesSumTotal;
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
        [ProducesResponseType(typeof(PaginationResponse<FinancialDebtDTO>), (int)HttpStatusCode.OK)]
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

        /// <summary>
        /// Make payment for debt
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("debt-payment")]
        public async Task<IActionResult> DebtPayment([FromBody] PayDebtCommand command)
        {
            await UpdateToken(command, nameof(PayDebtCommand));

            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Bill the client for all service rendenered on ticket and add cost to ticket
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPost("bill-client")]
        public async Task<IActionResult> BillClient([FromBody] BillClientCommand command)
        {
            await UpdateToken(command, nameof(BillClientCommand));

            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }


        /// <summary>
        /// Get the total sum of the bill
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpGet("get-total-bill")]
        public async Task<IActionResult> GetTotalBill([FromQuery] GetTicketInventoriesSumTotalQuery command)
        {
            await UpdateToken(command, nameof(GetTicketInventoriesSumTotalQuery));

            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }
    }
}

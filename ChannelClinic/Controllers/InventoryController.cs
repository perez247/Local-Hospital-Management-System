using Application.Command.InventoryEntities.AddInventoryDependencies;
using Application.Command.InventoryEntities.SaveInventory;
using Application.Command.InventoryEntities.SaveInventoryItem;
using Application.Command.InventoryEntities.SaveTicketInventory;
using Application.Command.TicketEntities.UpdateSurgeryTicket;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.InventoryEntities.GetInventories;
using Application.Query.InventoryEntities.GetInventoryItemAmount;
using Application.Query.InventoryEntities.GetInventoryItems;
using Application.Query.InventoryEntities.GetTicketInventories;
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
    public class InventoryController : BaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InventoryController(IMediator mediator, IUserRepository userRepository)
            : base(mediator, userRepository) { }

        /// <summary>
        /// Save an inventory
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(SaveInventoryResponse), (int)HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IActionResult> SaveInventory([FromBody] SaveInventoryCommand command)
        {
            await UpdateToken(command, nameof(SaveInventoryCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Save Inventory items
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<SaveInventoryItemResponse>), (int)HttpStatusCode.OK)]
        [HttpPost("inventory-item")]
        public async Task<IActionResult> SaveInventoryItem([FromBody] SaveInventoryItemCommand command)
        {
            await UpdateToken(command, nameof(SaveInventoryItemCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Get list of inventories
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PaginationResponse<IEnumerable<InventoryResponse>>), (int)HttpStatusCode.OK)]
        [HttpPost("inventories")]
        public async Task<IActionResult> GetInventories([FromBody] GetInventoriesQuery command)
        {
            await UpdateToken(command, nameof(GetInventoriesQuery));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Get list of inventory items
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PaginationResponse<IEnumerable<AppInventoryItemResponse>>), (int)HttpStatusCode.OK)]
        [HttpPost("inventory-items")]
        public async Task<IActionResult> GetInventoryItem([FromBody] GetInventoryItemsQuery command)
        {
            await UpdateToken(command, nameof(GetInventoryItemsQuery));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Get list of inventory items and prices based on the company
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PaginationResponse<IEnumerable<AppInventoryItemResponse>>), (int)HttpStatusCode.OK)]
        [HttpPost("inventory-item-prices")]
        public async Task<IActionResult> GetInventoryItemPrices([FromBody] GetInventoryItemAmountQuery command)
        {
            await UpdateToken(command, nameof(GetInventoryItemAmountQuery));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Update dependencies of an inventory
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PaginationResponse<IEnumerable<AppInventoryItemResponse>>), (int)HttpStatusCode.OK)]
        [HttpPut("dependencies")]
        public async Task<IActionResult> UpdateDependencies([FromBody] AddInventoryDependenciesCommand command)
        {
            await UpdateToken(command, nameof(AddInventoryDependenciesCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Update surgery ticket inventory
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(UpdateSurgeryTicketResponse), (int)HttpStatusCode.OK)]
        [HttpPut("surgery-inventory")]
        public async Task<IActionResult> UpdateSUrgeryInventory([FromBody] UpdateSurgeryTicketCommand command)
        {
            await UpdateToken(command, nameof(UpdateSurgeryTicketCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }


        /// <summary>
        /// Get ticket inventories
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PaginationResponse<IEnumerable<TicketInventoryResponse>>), (int)HttpStatusCode.OK)]
        [HttpPost("ticket-inventories")]
        public async Task<IActionResult> GetInventories([FromBody] GetTicketInventoriesQuery command)
        {
            await UpdateToken(command, nameof(GetTicketInventoriesQuery));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Update a ticket inventory
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApplicationErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApplicationBlankResponse), (int)HttpStatusCode.OK)]
        [HttpPut("ticket-inventory")]
        public async Task<IActionResult> UpdateTicketInventory([FromBody] SaveTicketInventoryCommand command)
        {
            await UpdateToken(command, nameof(SaveTicketInventoryCommand));
            var result = await ApplicationUserRequest?.Mediator?.Send(command);

            return Ok(result);
        }
    }
}

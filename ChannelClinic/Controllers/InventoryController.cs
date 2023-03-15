using Application.Command.SaveInventory;
using Application.Command.SaveInventoryItem;
using Application.Command.UpdateAppCost;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.GetInventories;
using Application.Query.GetInventoryItemAmount;
using Application.Query.GetInventoryItems;
using Application.RequestResponsePipeline;
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
    }
}

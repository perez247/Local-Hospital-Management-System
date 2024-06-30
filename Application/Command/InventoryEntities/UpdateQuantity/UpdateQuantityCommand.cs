using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.InventoryEntities.UpdateQuantity
{
    public class UpdateQuantityCommand: TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? AppInventoryId { get; set; }
        public bool? Add { get; set; }
        public int? Amount { get; set; }
        public string? Reason { get; set; }
    }

    public class UpdateQuantityHandler : IRequestHandler<UpdateQuantityCommand, Unit>
    {
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public UpdateQuantityHandler(IInventoryRepository IInventoryRepository, IDBRepository IDBRepository)
        {
            iInventoryRepository = IInventoryRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<Unit> Handle(UpdateQuantityCommand request, CancellationToken cancellationToken)
        {
            var inventory = await iInventoryRepository.AppInventories()
                                                      .FirstOrDefaultAsync(x => x.Id.ToString() == request.AppInventoryId);

            
            if (inventory == null)
            {
                throw new CustomMessageException("Inventory to update not found");
            }

            int oldQuantity = inventory.Quantity;

            if (request.Add.Value)
            {
                inventory.Quantity += request.Amount.Value;
            } else
            {
                if (inventory.Quantity < request.Amount.Value)
                {
                    throw new CustomMessageException($"{inventory.Quantity} is less than {request.Amount.Value}.");
                }

                inventory.Quantity = inventory.Quantity - request.Amount.Value;
            }

            var action = request.Add.Value ? "adding" : "subtracting" ;
            var newActivityLog = new ActivityLog
            {
                ActorId = request.getCurrentUserRequest().CurrentUser.Id,
                ActionType = nameof(UpdateQuantityCommand),
                ObjectType = nameof(AppInventory),
                ObjectId = inventory.Id.ToString(),
                ActionDescription = $"Update Quantity from {oldQuantity} to {inventory.Quantity} by {action} {request.Amount}",
                OtherDescription = request.Reason
            };

            iDBRepository.Update<AppInventory>(inventory);
            await iDBRepository.AddAsync<ActivityLog>(newActivityLog);

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}

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

namespace Application.Command.InventoryEntities.AddInventoryDependencies
{
    public class AddInventoryDependenciesCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? InventoryId { get; set; }
        public ICollection<AddInventoryDependenciesItem>? Dependencies { get; set; }
    }

    public class AddInventoryDependenciesHandler : IRequestHandler<AddInventoryDependenciesCommand, Unit>
    {
        private IInventoryRepository _inventoryRepository { get; set; }
        private IDBRepository _dBRepository { get; set; }

        public AddInventoryDependenciesHandler(IInventoryRepository inventoryRepository, IDBRepository dBRepository)
        {
            _inventoryRepository = inventoryRepository;
            _dBRepository = dBRepository;
        }

        public async Task<Unit> Handle(AddInventoryDependenciesCommand request, CancellationToken cancellationToken)
        {
            var inventory = await _inventoryRepository.AppInventories()
                                                      .Include(x => x.Dependencies)
                                                      .FirstOrDefaultAsync(x => x.Id.ToString() == request.InventoryId);

            if (inventory == null)
            {
                throw new CustomMessageException("Inventory not found");
            }

            var saved = false;

            // delete all dependencies if found;
            if (inventory.Dependencies.Count > 0) 
            { 
                _dBRepository.RemoveRange(inventory.Dependencies);
                saved = true;
            }

            // add new dependencies 

            if (request.Dependencies.Count > 0)
            {
                var newDependencies = request.Dependencies.Select(x => new AppInventoryDependencies
                {
                    AppInventoryId = Guid.Parse(x.InventoryId),
                    DefaultAmount = x.Amount,
                    DependantId = inventory.Id.Value
                });

                _dBRepository.AddRangeAsync(newDependencies);
                saved = true;
            }

            if (saved)
            {
                await _dBRepository.Complete();
            }

            return Unit.Value;
        }
    }
}

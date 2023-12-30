using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.InventoryEntities.BulkSave
{
    public class BulkSaveCommand: TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? CompanyId { get; set; }

        public ICollection<BulkSaveItemRequest>? Items { get; set; }
    }

    public class BulkSaveHandler : IRequestHandler<BulkSaveCommand, Unit>
    {
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public BulkSaveHandler(IInventoryRepository IInventoryRepository, IDBRepository IDBRepository)
        {
            iInventoryRepository = IInventoryRepository;
            iDBRepository = IDBRepository;
        }

        public async Task<Unit> Handle(BulkSaveCommand request, CancellationToken cancellationToken)
        {
            var items = request.Items.Select(x => x.FoundId).Where(a => a != Guid.Empty.ToString()).ToList();

            items = items.Distinct().ToList();

            var inventoryItems = new List<AppInventoryItem>();

            if (items.Count > 0)
            {
                inventoryItems = await iInventoryRepository.AppInventoryItems()
                                                        .Include(x => x.AppInventory)
                                                        .Where(x => items.Contains(x.Id.ToString()))
                                                        .ToListAsync();
            }

            var inventories = await iInventoryRepository.AppInventories()
                                                  .Where(x => request.Items.Select(a => a.InventoryId).Contains(x.Id.ToString()))
                                                  .ToListAsync();

            foreach (var item in request.Items)
            {
                if (item.FoundId != Guid.Empty.ToString())
                {
                    var itemInDb = inventoryItems.FirstOrDefault(x => x.Id.ToString() == item.FoundId);

                    if (itemInDb == null) 
                    {
                        throw new CustomMessageException($"{item.Name} was not found in the system, kindly enter it as a new item");
                    }

                    itemInDb.PricePerItem = item.Price.Value;

                    iDBRepository.Update<AppInventoryItem>(itemInDb);
                } else
                {
                    var newAppInventory = new AppInventory();

                    var appInventoryInDb = inventories.FirstOrDefault(x => x.Id.ToString() == item.InventoryId);

                    if (appInventoryInDb == null)
                    {
                        newAppInventory.Id = Guid.NewGuid();
                        newAppInventory.Name = item.Name;
                        newAppInventory.AppInventoryType = item.Type.ParseEnum<AppInventoryType>();
                        await iDBRepository.AddAsync(newAppInventory);
                    } else
                    {
                        newAppInventory = appInventoryInDb;
                    }

                    var newAppInventoryItem = new AppInventoryItem();
                    newAppInventoryItem.CompanyId = Guid.Parse(request.CompanyId);
                    newAppInventoryItem.AppInventoryId = newAppInventory.Id;
                    newAppInventoryItem.PricePerItem = item.Price.Value;
                    await iDBRepository.AddAsync(newAppInventoryItem);
                }
            }

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}

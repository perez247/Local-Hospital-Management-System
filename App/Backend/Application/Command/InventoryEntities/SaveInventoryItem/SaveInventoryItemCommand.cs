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

namespace Application.Command.InventoryEntities.SaveInventoryItem
{
    public class SaveInventoryItemCommand : TokenCredentials, IRequest<IEnumerable<SaveInventoryItemResponse>>
    {
        [VerifyGuidAnnotation]
        public string? InventoryId { get; set; }
        public bool? CreateIfNotFound { get; set; }
        public ICollection<SaveInventoryItemRequest>? InventoryItemRequests { get; set; }
    }

    public class SaveInventoryItemHandler : IRequestHandler<SaveInventoryItemCommand, IEnumerable<SaveInventoryItemResponse>>
    {
        private readonly IInventoryRepository iInventoryRepository;
        private readonly ICompanyRepository iCompanyRepository;
        private readonly IDBRepository iDBRepository;

        public SaveInventoryItemHandler(IInventoryRepository IInventoryRepository, ICompanyRepository ICompanyRepository, IDBRepository IDBRepository)
        {
            iInventoryRepository = IInventoryRepository;
            iCompanyRepository = ICompanyRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<IEnumerable<SaveInventoryItemResponse>> Handle(SaveInventoryItemCommand request, CancellationToken cancellationToken)
        {

            var requestToUpdate = request.InventoryItemRequests.DistinctBy(x => x.CompanyId).ToList();

            if (requestToUpdate.Count == 0)
            {
                throw new CustomMessageException("No Inventory items found to save");
            }

            var appInventoryFromDb = await iInventoryRepository.AppInventories().FirstOrDefaultAsync(x => x.Id.ToString() == request.InventoryId);

            if (appInventoryFromDb == null)
            {
                throw new CustomMessageException("Inventory not found");
            }

            var appInventoryItems = await iInventoryRepository.AppInventoryItems()
                                          .Where(a => a.AppInventoryId.ToString() == request.InventoryId && requestToUpdate.Select(x => x.CompanyId).Contains(a.CompanyId.ToString()))
                                          .ToListAsync();

            var companies = await iCompanyRepository.Companies()
                                    .Where(x => requestToUpdate.Select(x => x.CompanyId).Contains(x.Id.ToString()))
                                    .ToListAsync();


            if (companies.Count <= 0)
            {
                throw new CustomMessageException("No Companies found to save item, kindly add company first");
            }

            var responseList = new List<SaveInventoryItemResponse>();

            foreach (var req in requestToUpdate)
            {
                var appInv = await SaveInventoryItem(request, req, appInventoryItems);
                var appInvResponse = SaveInventoryItemResponse.Create(appInv);
                appInvResponse.Index = req.Index;
                responseList.Add(appInvResponse);
            }

            await iDBRepository.Complete();

            return responseList;
        }

        private async Task<AppInventoryItem> SaveInventoryItem(SaveInventoryItemCommand request, SaveInventoryItemRequest? req, List<AppInventoryItem>? appInventoryItems)
        {
            var inventoryItemInDb = appInventoryItems.FirstOrDefault(x => req.CompanyId == x.CompanyId.ToString());
            if (inventoryItemInDb == null)
            {
                var newInventoryItem = new AppInventoryItem();
                newInventoryItem.Id = Guid.NewGuid();
                newInventoryItem.PricePerItem = decimal.Round(req.CompanyAmount.Value, 2, MidpointRounding.AwayFromZero);
                newInventoryItem.CompanyId = Guid.Parse(req.CompanyId);
                newInventoryItem.AppInventoryId = Guid.Parse(request.InventoryId);
                await iDBRepository.AddAsync(newInventoryItem);
                return newInventoryItem;
            }
            else
            {
                inventoryItemInDb.PricePerItem = decimal.Round(req.CompanyAmount.Value, 2, MidpointRounding.AwayFromZero);
                iDBRepository.Update(inventoryItemInDb);
                return inventoryItemInDb;
            }
        }
    }
}

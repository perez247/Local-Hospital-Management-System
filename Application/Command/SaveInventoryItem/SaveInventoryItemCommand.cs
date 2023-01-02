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

namespace Application.Command.SaveInventoryItem
{
    public class SaveInventoryItemCommand : TokenCredentials, IRequest<IEnumerable<SaveInventoryItemResponse>>
    {
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
            var requestToUpdate = request.InventoryItemRequests.DistinctBy(x => x.InventoryId).ToList();

            if (requestToUpdate.Count == 0)
            {
                throw new CustomMessageException("No Inventory item found to save");
            }

            var companies = await iCompanyRepository.Companies()
                                           .Where(a => requestToUpdate.Select(x => x.CompanyId).Contains(a.Id.ToString()))
                                           .ToListAsync();

            var appInventories = await iInventoryRepository.AppInventories()
                                           .Where(a => requestToUpdate.Select(x => x.InventoryId).Contains(a.Id.ToString()))
                                           .ToListAsync();

            var responseList = new List<SaveInventoryItemResponse>();

            foreach (var req in requestToUpdate)
            {
                var appInv = await SaveInventoryItem(req, companies, appInventories);
                var appInvResponse = SaveInventoryItemResponse.Create(appInv);
                appInvResponse.Index = req.Index;
                responseList.Add(appInvResponse);
            }

            return responseList;
        }

        private async Task<AppInventoryItem> SaveInventoryItem(SaveInventoryItemRequest? req, List<Company>? companies, List<AppInventory>? appInventories)
        {
            if (req.InventoryItemId == Guid.Empty.ToString())
            {
                var companyId = GetCompanyid(req, companies);
                var appInventory = GetInventory(req, appInventories);

                var newInventoryItem = new AppInventoryItem();
                newInventoryItem.Id = Guid.NewGuid();
                newInventoryItem.PricePerItem = req.NewPrice.Value;
                newInventoryItem.CompanyId = companyId;
                newInventoryItem.AppInventoryId = appInventory;
                await iDBRepository.AddAsync<AppInventoryItem>(newInventoryItem);
                return newInventoryItem;
            }
            else
            {
                var inventorytemInDb = await iInventoryRepository.AppInventoryItems()
                                                                 .FirstOrDefaultAsync(x => x.Id.ToString() == req.InventoryItemId);

                if (inventorytemInDb == null)
                {
                    throw new CustomMessageException("Inventory Item not found");
                }

                inventorytemInDb.PricePerItem = req.NewPrice.Value;
                iDBRepository.Update<AppInventoryItem>(inventorytemInDb);
                return inventorytemInDb;
            }
        }

        private Guid? GetCompanyid(SaveInventoryItemRequest? req, List<Company>? companies)
        {
            if (req.CompanyId != Guid.Empty.ToString())
            {
                var company = companies.FirstOrDefault(x => x.Id.ToString() == req.CompanyId);

                if (company == null)
                {
                    throw new CustomMessageException("Company not found");
                }
                return company.Id;
            }
            return null;
        }

        private Guid? GetInventory(SaveInventoryItemRequest? req, List<AppInventory>? appInventories)
        {
            var appInventory = appInventories.FirstOrDefault(x => x.Id.ToString() == req.InventoryId);

            if (appInventory == null)
            {
                throw new CustomMessageException("Inventory not found");
            }
            return appInventory.Id;
        }
    }
}

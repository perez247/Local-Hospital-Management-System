using Application.Paginations;
using Application.Query.InventoryEntities.GetInventories;
using Application.Query.InventoryEntities.GetInventoryItems;
using Application.Query.InventoryEntities.GetTicketInventories;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IInventoryRepository
    {
        IQueryable<AppInventory> AppInventories();
        IQueryable<AppInventoryItem> AppInventoryItems();
        IQueryable<TicketInventory> TicketInventories();
        Task<PaginationDto<AppInventory>> GetInventoryList(GetInventoriesFilter filter, PaginationCommand command);
        Task<PaginationDto<AppInventoryItem>> GetInventoryItemList(GetInventoryItemFilter filter, PaginationCommand command);
        Task<PaginationDto<TicketInventory>> GetTickeyInventories(GetTicketInventoriesFilter filter, PaginationCommand command);
        Task<PaginationDto<AppInventoryItem>> SearchTickeyInventoriesByName(string companyId, List<string>? names);

    }
}

using Application.Paginations;
using Application.Query.GetInventories;
using Application.Query.GetInventoryItems;
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
        Task<PaginationDto<AppInventory>> GetInventoryList(GetInventoriesFilter filter, PaginationCommand command);
        Task<PaginationDto<AppInventoryItem>> GetInventoryItemList(GetInventoryItemFilter filter, PaginationCommand command);
    }
}

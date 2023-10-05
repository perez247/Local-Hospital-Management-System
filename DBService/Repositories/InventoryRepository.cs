using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.InventoryEntities.GetInventories;
using Application.Query.InventoryEntities.GetInventoryItems;
using Application.Query.InventoryEntities.GetTicketInventories;
using DBService.QueryHelpers;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDBContext _context;
        public InventoryRepository(AppDBContext context)
        {
            _context = context;
        }

        public IQueryable<AppInventory> AppInventories()
        {
            return _context.AppInventories.AsQueryable();
        }

        public IQueryable<AppInventoryItem> AppInventoryItems()
        {
            return _context.AppInventoryItems.AsQueryable();
        }

        public async Task<PaginationDto<AppInventory>> GetInventoryList(GetInventoriesFilter filter, PaginationCommand command)
        {
            var query = _context.AppInventories
                                .Include(x => x.Dependencies)
                                    .ThenInclude(x => x.AppInventory)
                                .OrderByDescending(x => x.DateCreated)
                                .AsQueryable();

            query = InventoryQueryHelper.FilterInventory(query, filter);

            return await query.GenerateEntity(command);
        }

        public async Task<PaginationDto<AppInventoryItem>> GetInventoryItemList(GetInventoryItemFilter filter, PaginationCommand command)
        {
            var query = _context.AppInventoryItems
                                .Include(x => x.Company)
                                    .ThenInclude(x => x.AppUser)
                                .Include(x => x.AppInventory)
                                .OrderByDescending(x => x.DateCreated)
                                .AsQueryable();

            query = InventoryQueryHelper.FilterInventoryItem(query, filter);

            return await query.GenerateEntity(command);
        }

        public async Task<PaginationDto<TicketInventory>> GetTickeyInventories(GetTicketInventoriesFilter filter, PaginationCommand command)
        {
            var query = _context.TicketInventories
                                .Include(x => x.AppTicket)
                                .Include(x => x.AdmissionPrescription)
                                .Include(x => x.AppInventory)
                                .Include(x => x.Staff)
                                    .ThenInclude(x => x.AppUser)
                                .OrderByDescending(x => x.DateCreated)
                                .AsQueryable();

            query = InventoryQueryHelper.FilterTicketInventory(query, filter);

            return await query.GenerateEntity(command);
        }
    }
}

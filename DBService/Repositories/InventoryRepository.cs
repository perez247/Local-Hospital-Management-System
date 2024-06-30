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
using System.Text.RegularExpressions;
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

        public IQueryable<TicketInventory> TicketInventories()
        {
            return _context.TicketInventories.AsQueryable();
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
                                .Include(x => x.TicketInventoryDebtors)
                                    .ThenInclude(x => x.Payer)
                                .Include(x => x.Staff)
                                    .ThenInclude(x => x.AppUser)
                                .Include(x => x.SurgeryTicketPersonnels)
                                    .ThenInclude(x => x.Personnel)
                                .OrderByDescending(x => x.DateCreated)
                                .AsQueryable();

            query = InventoryQueryHelper.FilterTicketInventory(query, filter);

            return await query.GenerateEntity(command);
        }

        public async Task<PaginationDto<AppInventoryItem>> SearchTickeyInventoriesByName(string companyId, List<string>? names)
        {
            string pattern = @"^\w+";
            var lowerNames = names.Select(x => Regex.Match(x.ToLower(), pattern));
            var searchConditions = string.Join(" OR ", lowerNames.Select(term => $"LOWER(\"AppInventories\".\"Name\") ILIKE '%{term}%'"));
            var q = $"SELECT \"AppInventoryItems\".* FROM \"AppInventoryItems\" INNER JOIN \"AppInventories\" ON \"AppInventoryItems\".\"AppInventoryId\" = \"AppInventories\".\"Id\" WHERE({searchConditions}) AND \"AppInventoryItems\".\"CompanyId\" = '{companyId}'";

            var query = await _context.AppInventoryItems
                                .FromSqlRaw(q)
                                .Include(x => x.AppInventory)
                                .ToListAsync();

            var inventoryQ = $"SELECT \"AppInventories\".* FROM \"AppInventories\" WHERE({searchConditions})";
            var inventoryQuery = await _context.AppInventories
                                               .FromSqlRaw(inventoryQ)
                                               .ToListAsync();

            var notFoundInQuery = inventoryQuery.Where(x => !query.Select(a => a.AppInventoryId).Contains(x.Id));

            if (notFoundInQuery.Count() > 0)
            {
                var notFoundItems = notFoundInQuery.Select(x => new AppInventoryItem
                {
                    Company = null,
                    PricePerItem = 0,
                    AppInventory = x,
                });

                query = query.Concat(notFoundItems).ToList();
            }

            return new PaginationDto<AppInventoryItem>
            {
                totalItems = query.Count(),
                Results = query,
                PageNumber = 1,
                PageSize = 100,
            };
        }
    }
}

using Application.Query.InventoryEntities.GetInventories;
using Application.Query.InventoryEntities.GetInventoryItems;
using Application.Query.StaffPaymentHistory;
using Application.Utilities;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.QueryHelpers
{
    public static class InventoryQueryHelper
    {
        public static IQueryable<AppInventory> FilterInventory(IQueryable<AppInventory> query, GetInventoriesFilter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(i => EF.Functions.Like(i.Name.ToLower(), $"%{filter.Name.ToLower()}%"));
            }

            if (filter.AppInventoryType != null && filter.AppInventoryType.Count > 0)
            {
                var type = filter.AppInventoryType.Select(x => x.ParseEnum<AppInventoryType>());
                query = query.Where(x => type.Contains(x.AppInventoryType));
            }

            if (!string.IsNullOrEmpty(filter.Quantity))
            {
                int amount;
                int.TryParse(filter.Quantity, out amount);

                query = query.Where(x => x.Quantity <= amount);
            }

            if (filter.InventoryId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.Id.ToString() == filter.InventoryId);
            }


            return query;
        }


        public static IQueryable<AppInventoryItem> FilterInventoryItem(IQueryable<AppInventoryItem> query, GetInventoryItemFilter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (filter.Amount.HasValue)
            {
                query = query.Where(x => x.PricePerItem <= filter.Amount.Value);
            }

            if (filter.CompanyId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.Company.Id.ToString() == filter.CompanyId);
            }

            if (filter.AppInventoryId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.AppInventoryId.HasValue && x.AppInventoryId.ToString() == filter.AppInventoryId);
            }

            if (!string.IsNullOrEmpty(filter.AppInventoryType))
            {
                var type = filter.AppInventoryType.ParseEnum<AppInventoryType>();
                query = query.Where(x => x.AppInventory.AppInventoryType == type);
            }

            if (!string.IsNullOrEmpty(filter.CompanyName))
            {
                var name = filter.CompanyName.Trim();
                query = query.Where(i => EF.Functions.Like(i.Company.AppUser.FirstName.ToLower(), $"%{name.ToLower()}%"));
            }

            if (!string.IsNullOrEmpty(filter.AppInventoryName))
            {
                var name = filter.AppInventoryName.Trim();
                query = query.Where(i => EF.Functions.Like(i.AppInventory.Name.ToLower(), $"%{name.ToLower()}%"));
            }

            return query;
        }
    }
}

using Application.Query.GetStaffList;
using Application.Query.StaffPaymentHistory;
using Application.Responses;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.QueryHelpers
{
    public static class StaffQueryHelper
    {
        public static IQueryable<AppUser> FilterStaffList(IQueryable<AppUser> query, GetStaffListFilter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(i => 
                    (!string.IsNullOrEmpty(i.FirstName) && EF.Functions.Like(i.FirstName.ToLower(), $"%{filter.Name.ToLower()}%")) ||
                    (!string.IsNullOrEmpty(i.LastName) && EF.Functions.Like(i.LastName.ToLower(), $"%{filter.Name.ToLower()}%")) ||
                    (!string.IsNullOrEmpty(i.OtherName) && EF.Functions.Like(i.OtherName.ToLower(), $"%{filter.Name.ToLower()}%")) 
                );

            if (filter.Active.HasValue)
            {
                query = query.Where(i => i.Staff != null && i.Staff.Active == filter.Active.Value);
            }

            return query;
        }

        public static IQueryable<SalaryPaymentHistory> FilterPaymentHistory(IQueryable<SalaryPaymentHistory> query, StaffPaymentHistoryFilter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (filter.Date.HasValue)
            {
                query = query.Where(x => x.DatePaidFor.Year == filter.Date.Value.Year && x.DatePaidFor.Month == filter.Date.Value.Month);
            }


            if (filter.Paid.HasValue)
            {
                query = query.Where(x => x.Paid == filter.Paid.Value);
            }

            return query;
        }
    }
}

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

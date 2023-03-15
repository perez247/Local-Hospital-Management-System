using Application.Query.GetPendingUserContracts;
using Application.Query.GetTickets;
using Application.Utilities;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.QueryHelpers
{
    public static class FinancialQueryHelper
    {
        public static IQueryable<AppCost> FilterAppCost(IQueryable<AppCost> query, GetPendingUserContractsFilter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (!string.IsNullOrEmpty(filter.PaymentStatus))
            {
                var type = filter.PaymentStatus.ParseEnum<PaymentStatus>();
                query = query.Where(x => x.PaymentStatus == type);
            }

            if (filter.Patient)
            {
                query = query.Where(x => x.PatientContract != null);
            }

            if (filter.Company)
            {
                query = query.Where(x => x.CompanyContract != null);
            }

            return query;
        }
    }
}

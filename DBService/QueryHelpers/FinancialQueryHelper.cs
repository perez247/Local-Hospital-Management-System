using Application.Query.FinancialRecordEntities.GetAppCosts;
using Application.Query.FinancialRecordEntities.GetFinancialRecords;
using Application.Query.FinancialRecordEntities.GetPendingUserContracts;
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

        public static IQueryable<AppCost> FilterAppCostDebt(IQueryable<AppCost> query, GetAppCostFilter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (filter.StartDate.HasValue)
            {
                query = query.Where(x => x.DateCreated >= filter.StartDate.Value);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(x => x.DateCreated <= filter.EndDate.Value);
            }

            if (filter.PatientId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.FinancialRecordPayerPayees.Select(x => x.AppUserId.ToString()).Contains(filter.PatientId));
            }

            if (filter.CompanyId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.FinancialRecordPayerPayees.Select(x => x.AppUserId.ToString()).Contains(filter.CompanyId));
            }

            return query;
        }


        public static IQueryable<FinancialRecord> FilterAppCostPaid(IQueryable<FinancialRecord> query, GetFinancialRecordsFilter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (filter.StartDate.HasValue)
            {
                query = query.Where(x => x.DateCreated >= filter.StartDate.Value);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(x => x.DateCreated <= filter.EndDate.Value);
            }

            if (filter.PatientId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.FinancialRecordPayerPayees.Select(x => x.AppUserId.ToString()).Contains(filter.PatientId));
            }

            if (filter.CompanyId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.FinancialRecordPayerPayees.Select(x => x.AppUserId.ToString()).Contains(filter.CompanyId));
            }

            return query;
        }
    }
}

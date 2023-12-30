using Application.DTOs;
using Application.Paginations;
using Application.Query.FinancialRecordEntities.GetAppCosts;
using Application.Query.FinancialRecordEntities.GetFinancialRecords;
using Application.Query.FinancialRecordEntities.GetPendingUserContracts;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IFinancialRespository 
    {
        IQueryable<FinancialRecord> FinancialRecords();
        IQueryable<MonthlyFinanceRecord> MonthlyFinancialRecords();
        Task<decimal> GetTax();
        Task<decimal> CompanyContractCost();
        Task<decimal> GetPatientContractCost();
        Task<PaginationDto<AppCost>> GetContracts(GetPendingUserContractsFilter filter, PaginationCommand command);
        IQueryable<AppCost> GetAppCosts();
        Task<PaginationDto<FinancialDebtDTO>> GetAppCostForDebts(GetAppCostFilter filter, PaginationCommand command);
        Task<PaginationDto<FinancialRecord>> GetFinancialRecords(GetFinancialRecordsFilter filter, PaginationCommand command);
    }
}

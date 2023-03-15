using Application.Paginations;
using Application.Query.GetPendingUserContracts;
using Application.Query.GetTickets;
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
        Task<decimal> GetTax();
        Task<decimal> CompanyContractCost();
        Task<decimal> GetPatientContractCost();
        Task<PaginationDto<AppCost>> GetContracts(GetPendingUserContractsFilter filter, PaginationCommand command);
        IQueryable<AppCost> GetAppCosts();
    }
}

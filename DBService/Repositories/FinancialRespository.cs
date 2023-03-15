using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.GetPendingUserContracts;
using Application.Query.GetTickets;
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
    public class FinancialRespository : IFinancialRespository
    {
        private readonly AppDBContext _context;
        public FinancialRespository(AppDBContext context)
        {
            _context = context;
        }

        public IQueryable<FinancialRecord> FinancialRecords()
        {
            return _context.FinancialRecords.AsQueryable();
        }

        public async Task<decimal> GetTax()
        {
            await Task.Delay(500);
            return (decimal)0.01;
        }

        public async Task<decimal> CompanyContractCost()
        {
            await Task.Delay(500);
            return (decimal)10000;
        }

        public async Task<decimal> GetPatientContractCost()
        {
            await Task.Delay(500);
            return (decimal)5000;
        }

        public async Task<PaginationDto<AppCost>> GetContracts(GetPendingUserContractsFilter filter, PaginationCommand command)
        {
            var query = _context.AppCosts
                                .Include(x => x.PatientContract)
                                    .ThenInclude(x => x.Patient)
                                        .ThenInclude(x => x.AppUser)
                                .Include(x => x.CompanyContract)
                                    .ThenInclude(x => x.Company)
                                        .ThenInclude(x => x.AppUser)
                                .Where(x => x.PatientContract != null || x.CompanyContract != null)
                                .AsQueryable();

            query = FinancialQueryHelper.FilterAppCost(query, filter);

            return await query.GenerateEntity(command);
        }

        public IQueryable<AppCost> GetAppCosts()
        {
            return _context.AppCosts;
        }

    }
}

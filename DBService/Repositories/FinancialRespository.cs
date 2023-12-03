using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.FinancialRecordEntities.GetAppCosts;
using Application.Query.FinancialRecordEntities.GetFinancialRecords;
using Application.Query.FinancialRecordEntities.GetPendingUserContracts;
using DBService.QueryHelpers;
using Microsoft.EntityFrameworkCore;
using Models;
using Newtonsoft.Json;
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
            var data = await GetBilling();
            return data.Tax;
        }

        public async Task<decimal> CompanyContractCost()
        {
            var data = await GetBilling();
            return data.CompanyRegistrationFee;
        }

        public async Task<decimal> GetPatientContractCost()
        {
            var data = await GetBilling();
            return data.PatientRegistrationFee;
        }

        private async Task<AppSettingBilling> GetBilling()
        {
            var billing = await _context.AppSettings.FirstOrDefaultAsync(x => x.Type == Models.Enums.AppSettingType.billings);

            if (billing == null)
            {
                throw new CustomMessageException("Settings not found");
            }

            return JsonConvert.DeserializeObject<AppSettingBilling>(billing.Data);
        }

        public async Task<PaginationDto<AppCost>> GetContracts(GetPendingUserContractsFilter filter, PaginationCommand command)
        {
            var query = _context.AppCosts
                                .Include(x => x.PatientContract)
                                    .ThenInclude(x => x.Patient)
                                .Include(x => x.CompanyContract)
                                    .ThenInclude(x => x.Company)
                                .Include(x => x.FinancialRecordPayerPayees)
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

        public async Task<PaginationDto<FinancialDebtDTO>> GetAppCostForDebts(GetAppCostFilter filter, PaginationCommand command)
        {
            var query = _context.AppCosts
                                .Include(x => x.AppTicket)
                                    .ThenInclude(x => x.TicketInventories)
                                        .ThenInclude(x => x.AppInventory)
                                .Include(x => x.AppTicket)
                                    .ThenInclude(x => x.Appointment)
                                        .ThenInclude(x => x.Patient)
                                        .ThenInclude(x => x.AppUser)
                                .Include(x => x.PatientContract)
                                    .ThenInclude(x => x.Patient)
                                .Include(x => x.CompanyContract)
                                    .ThenInclude(x => x.Company)
                                .Include(x => x.FinancialRecordPayerPayees)
                                    .ThenInclude(x => x.AppUser)
                                        .ThenInclude(x => x.Patient)
                                .Include(x => x.FinancialRecordPayerPayees)
                                    .ThenInclude(x => x.AppUser)
                                        .ThenInclude(x => x.Company)
                                .Where(x => x.PaymentStatus == Models.Enums.PaymentStatus.owing && x.PatientContract == null)
                                .OrderByDescending(x => x.DateCreated)
                                .AsQueryable();

            query = FinancialQueryHelper.FilterAppCostDebt(query, filter);

            var data = await query.GenerateEntity(command);

            //var debt = query.SumAsync(x => x.ApprovedPrice);
            var paid =  await query.Select(x => x.Payments).ToListAsync();
            var totalPaid = paid.Sum(x => x.Sum(y => y.Amount));

            var newData = new FinancialDebtDTO
            {
                AppCosts = data.Results,
                Debt = await query.SumAsync(x => x.ApprovedPrice) ?? 0,
                Paid = totalPaid
            };

            return new PaginationDto<FinancialDebtDTO>
            {
                Results = new List<FinancialDebtDTO>() { newData },
                PageNumber = data.PageNumber,
                PageSize = data.PageSize,
                totalItems = data.totalItems
            };
        }

        public async Task<PaginationDto<FinancialRecord>> GetFinancialRecords(GetFinancialRecordsFilter filter, PaginationCommand command)
        {
            var query = _context.FinancialRecords
                                .Include(x => x.FinancialRecordPayerPayees)
                                    .ThenInclude(x => x.AppUser)
                                        .ThenInclude(x => x.Patient)
                                .Include(x => x.FinancialRecordPayerPayees)
                                    .ThenInclude(x => x.AppUser)
                                        .ThenInclude(x => x.Company)
                                .Include(x => x.AppCosts)
                                .Select(x => new FinancialRecord
                                        {
                                            Id = x.Id,
                                            DateCreated = x.DateCreated,
                                            DateModified = x.DateModified,
                                            Amount = x.Amount,
                                            ApprovedAmount = x.ApprovedAmount,
                                            CostType = x.CostType,
                                            Payments = x.Payments,
                                            PaymentStatus = x.PaymentStatus,
                                            Description = x.Description,
                                            TotalAppCosts = x.AppCosts.Count(),
                                            FinancialRecordPayerPayees = x.FinancialRecordPayerPayees,
                                            FinancialRequest = x.FinancialRequest,
                                        })
                                .OrderByDescending(x => x.DateCreated)
                                //.Where(x => x.PaymentStatus == Models.Enums.PaymentStatus.approved)
                                .AsQueryable();

            query = FinancialQueryHelper.FilterAppCostPaid(query, filter);

            return await query.GenerateEntity(command);
        }

    }
}

﻿using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.FinancialRecordEntities.GetAppCosts;
using Application.Query.FinancialRecordEntities.GetFinancialRecords;
using Application.Query.FinancialRecordEntities.GetPendingUserContracts;
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

        public async Task<PaginationDto<AppCost>> GetAppCostForDebts(GetAppCostFilter filter, PaginationCommand command)
        {
            var query = _context.AppCosts
                                .Include(x => x.AppTicket)
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

            return await query.GenerateEntity(command);
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
                                .AsQueryable();

            query = FinancialQueryHelper.FilterAppCostPaid(query, filter);

            return await query.GenerateEntity(command);
        }

    }
}

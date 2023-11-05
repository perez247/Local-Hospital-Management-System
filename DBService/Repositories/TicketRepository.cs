using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.AdmissionEntities.GetPrescriptions;
using Application.Query.TicketEntities.GetTickets;
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
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDBContext _context;
        public TicketRepository(AppDBContext context)
        {
            _context = context;
        }
        public IQueryable<AppTicket> AppTickets()
        {
            return _context.AppTickets.AsQueryable();
        }
        public IQueryable<TicketInventory> TicketInventory()
        {
            return _context.TicketInventories.AsQueryable();
        }

        public IQueryable<AdmissionPrescription> TicketPrescription()
        {
            return _context.AdmissionPrescriptions.AsQueryable();
        }

        public async Task<PaginationDto<AppTicket>> GetTickets(GetTicketsQueryFilter filter, PaginationCommand command)
        {
            var query = _context.AppTickets
                                .Include(x => x.AppCost)
                                    .ThenInclude(x => x.FinancialRecordPayerPayees)
                                        .ThenInclude(x => x.AppUser)
                                            .ThenInclude(x => x.Patient)
                                .Include(x => x.AppCost)
                                    .ThenInclude(x => x.FinancialRecordPayerPayees)
                                        .ThenInclude(x => x.AppUser)
                                            .ThenInclude(x => x.Company)
                                //.Include(x => x.Patient)
                                //    .ThenInclude(x => x.AppUser)
                                //.Include(x => x.Company)
                                //    .ThenInclude(x => x.AppUser)
                                .Include(x => x.TicketInventories)
                                    .ThenInclude(x => x.AppInventory)
                                .Include(x => x.TicketInventories)
                                    .ThenInclude(x => x.SurgeryTicketPersonnels)
                                .ThenInclude(x => x.Personnel)
                                .Include(x => x.Appointment)
                                    .ThenInclude(x => x.Patient)
                                        .ThenInclude(x => x.AppUser)
                                .Include(x => x.Appointment)
                                    .ThenInclude(x => x.Patient)
                                        .ThenInclude(x => x.PatientContracts.OrderByDescending(a => a.DateCreated).Take(1))
                                .Include(x => x.Appointment)
                                    .ThenInclude(x => x.Doctor)
                                        .ThenInclude(x => x.AppUser)
                                .Include(x => x.Appointment)
                                    .ThenInclude(x => x.Patient)
                                        .ThenInclude(x => x.Company)
                                            .ThenInclude(x => x.AppUser)
                                .Include(x => x.Appointment)
                                    .ThenInclude(x => x.Patient)
                                        .ThenInclude(x => x.Company)
                                            .ThenInclude(x => x.CompanyContracts.OrderByDescending(a => a.DateCreated).Take(1))
                                .OrderByDescending(x => x.DateCreated)
                                .AsQueryable();

            query = TicketQueryHelper.FilterTicket(query, filter);

            return await query.GenerateEntity(command);
        }

        public async Task<PaginationDto<AppTicket>> GetLinerTickets(GetTicketsQueryFilter filter, PaginationCommand command)
        {
            var query = _context.AppTickets
                                .Include(x => x.AppCost)
                                .Include(x => x.Appointment)
                                    .ThenInclude(x => x.Patient)
                                        .ThenInclude(x => x.AppUser)
                                .Include(x => x.Appointment)
                                    .ThenInclude(x => x.Patient)
                                        .ThenInclude(x => x.Company)
                                            .ThenInclude(x => x.AppUser)
                                .Include(x => x.Appointment)
                                    .ThenInclude(x => x.Doctor)
                                        .ThenInclude(x => x.AppUser)
                                .OrderByDescending(x => x.DateCreated)
                                .AsQueryable();

            query = TicketQueryHelper.FilterTicket(query, filter);

            return await query.GenerateEntity(command);
        }

        public async Task<PaginationDto<AdmissionPrescription>> GetAdmissionPrescription(GetPrescriptionQueryFilter filter, PaginationCommand command)
        {
            var query = _context.AdmissionPrescriptions
                                .Include(x => x.Doctor)
                                    .ThenInclude(x=> x.Staff)
                                .Include(x => x.TicketInventories.Where(x => !x.AppTicketId.HasValue))
                                    .ThenInclude(x => x.AppInventory)
                                .OrderByDescending(x => x.DateCreated)
                                .AsQueryable();

            query = TicketQueryHelper.FilterAdmissionPrescription(query, filter);

            return await query.GenerateEntity(command);
        }
    }
}
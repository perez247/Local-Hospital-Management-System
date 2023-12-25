using Application.Paginations;
using Application.Query.AdmissionEntities.GetPrescriptions;
using Application.Query.TicketEntities.GetTickets;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface ITicketRepository
    {
        IQueryable<AppTicket> AppTickets();
        IQueryable<TicketInventory> TicketInventory();
        IQueryable<AdmissionPrescription> TicketPrescription();
        Task<PaginationDto<AppTicket>> GetTickets(GetTicketsQueryFilter filter, PaginationCommand command);
        Task<PaginationDto<AppTicket>> GetLinerTickets(GetTicketsQueryFilter filter, PaginationCommand command);
        Task<PaginationDto<AdmissionPrescription>> GetAdmissionPrescription(GetPrescriptionQueryFilter filter, PaginationCommand command);
    }
}

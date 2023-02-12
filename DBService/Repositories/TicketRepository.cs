using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.GetInventoryItems;
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
        public async Task<PaginationDto<AppTicket>> GetTickets(GetTicketsQueryFilter filter, PaginationCommand command)
        {
            var query = _context.AppTickets
                                .Include(x => x.AppCost)
                                .Include(x => x.TicketInventories)
                                    .ThenInclude(x => x.AppInventory)
                                .Include(x => x.TicketInventories)
                                    .ThenInclude(x => x.SurgeryTicketPersonnels)
                                        .ThenInclude(x => x.Personnel)
                                .OrderByDescending(x => x.DateCreated)
                                .AsQueryable();

            query = TicketQueryHelper.FilterTicket(query, filter);

            return await query.GenerateEntity(command);
        }
    }
}
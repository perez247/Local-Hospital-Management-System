using Application.Interfaces.IRepositories;
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
    }
}
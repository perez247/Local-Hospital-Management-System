using Application.Interfaces.IRepositories;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDBContext _context;
        public AppointmentRepository(AppDBContext context)
        {
            _context = context;
        }
        public IQueryable<AppAppointment> AppAppointments()
        {
            return _context.AppAppointments.AsQueryable();
        }
    }
}

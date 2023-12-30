using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.AppointmentEntities.GetAppointments;
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
        public async Task<PaginationDto<AppAppointment>> GetAppointmentByDate(GetAppoinmentFilter filter, PaginationCommand command)
        {
            var query = _context.AppAppointments
                                .Include(x => x.Company).ThenInclude(x => x.AppUser)
                                .Include(x => x.Patient).ThenInclude(x => x.AppUser)
                                .Include(x => x.Doctor).ThenInclude(x => x.AppUser)
                                .OrderByDescending(x => x.AppointmentDate)
                                .AsQueryable();

            query = AppointmentQueryHandler.FilterAppointmentByDate(query, filter);

            return await query.GenerateEntity(command);
        }
    }
}

using Application.Paginations;
using Application.Query.GetAppointments;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IAppointmentRepository
    {
        IQueryable<AppAppointment> AppAppointments();
        Task<PaginationDto<AppAppointment>> GetAppointmentByDate(GetAppoinmentFilter filter, PaginationCommand command);
    }
}

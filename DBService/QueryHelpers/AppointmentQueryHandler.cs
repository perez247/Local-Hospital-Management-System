using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Query.AppointmentEntities.GetAppointments;

namespace DBService.QueryHelpers
{
    public static class AppointmentQueryHandler
    {
        public static IQueryable<AppAppointment> FilterAppointmentByDate(IQueryable<AppAppointment> query, GetAppoinmentFilter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (filter.ExactDate.HasValue)
            {
                filter.ExactDate = filter.ExactDate.Value.ToUniversalTime();
                query = query.Where(i => i.AppointmentDate.Year == filter.ExactDate.Value.Year && i.AppointmentDate.Month == filter.ExactDate.Value.Month && i.AppointmentDate.Day == filter.ExactDate.Value.Day);
            }

            if (filter.StartDate.HasValue)
            {
                var date = new DateTime(filter.StartDate.Value.Year, filter.StartDate.Value.Month, filter.StartDate.Value.Day, 0, 0, 0).ToUniversalTime();
                query = query.Where(i => i.AppointmentDate >= date);
            }

            if (filter.AppointmentId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.Id.ToString() == filter.AppointmentId);
            }

            if (filter.DoctorId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.DoctorId.ToString() == filter.DoctorId);
            }

            if (filter.PatientId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.PatientId.ToString() == filter.PatientId);
            }

            return query;
        }
    }
}

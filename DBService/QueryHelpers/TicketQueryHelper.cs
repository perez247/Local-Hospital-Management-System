using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Utilities;
using Application.Query.TicketEntities.GetTickets;

namespace DBService.QueryHelpers
{
    public static class TicketQueryHelper
    {
        public static IQueryable<AppTicket> FilterTicket(IQueryable<AppTicket> query, GetTicketsQueryFilter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (filter.AppointmentId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.AppointmentId.ToString() == filter.AppointmentId);
            }

            if (!string.IsNullOrEmpty(filter.AppInventoryType))
            {
                var type = filter.AppInventoryType.ParseEnum<AppInventoryType>();
                query = query.Where(x => x.AppInventoryType == type);
            }

            if (filter.PatientId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.Appointment.Patient.AppUserId.ToString() == filter.PatientId);
            }

            if (filter.DoctorId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.Appointment.Doctor.AppUserId.ToString() == filter.DoctorId);
            }

            if (filter.TicketId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.Id.ToString() == filter.TicketId);
            }

            if (filter.SentToDepartment.HasValue)
            {
                query = query.Where(x => x.Sent.Value == filter.SentToDepartment.Value);
            }

            if (filter.SentToFinance.HasValue)
            {
                query = query.Where(x => x.SentToFinance.Value == filter.SentToFinance);
            }

            if (!string.IsNullOrEmpty(filter.AppTicketStatus))
            {
                var type = filter.AppTicketStatus.ParseEnum<AppTicketStatus>();
                query = query.Where(x => x.AppTicketStatus == type);
            }

            return query;
        }

    }
}

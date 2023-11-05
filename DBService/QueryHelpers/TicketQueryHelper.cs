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
using Application.Query.AdmissionEntities.GetPrescriptions;

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
                query = query.Where(x => x.Appointment.PatientId.ToString() == filter.PatientId);
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

            if (filter.BeforeDateTime.HasValue)
            {
                query = query.Where(x => filter.BeforeDateTime.Value >= x.DateCreated);
            }

            if (!string.IsNullOrEmpty(filter.AppTicketStatus))
            {
                var type = filter.AppTicketStatus.ParseEnum<AppTicketStatus>();
                query = query.Where(x => x.AppTicketStatus == type);
            }

            if (filter.PaymentStatus != null && filter.PaymentStatus.Count > 0)
            {
                var type = filter.PaymentStatus.Select(x => x.ParseEnum<PaymentStatus>());
                query = query.Where(x => x.AppCost == null || type.Contains(x.AppCost.PaymentStatus));
            }

            return query;

        }

        public static IQueryable<AdmissionPrescription> FilterAdmissionPrescription(IQueryable<AdmissionPrescription> query, GetPrescriptionQueryFilter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (filter.TicketId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.AppTicketId.ToString() == filter.TicketId);
            }

            if (filter.PrescriptionId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.Id.ToString() == filter.PrescriptionId);
            }

            if (!string.IsNullOrEmpty(filter.AppInventoryType))
            {
                var type = filter.AppInventoryType.ParseEnum<AppInventoryType>();
                query = query.Where(x => x.AppInventoryType == type);
            }

            return query;

        }

    }
}

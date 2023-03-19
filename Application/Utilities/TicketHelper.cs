using Application.Command.AppointmentEntities.AddAppointment;
using Application.Command.CreateTicket;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities
{
    public static class TicketHelper
    {
        public static async Task<AppAppointment> CreateAppointment(AppAppointmentCommand request, IStaffRepository iStaffRepository, IPatientRepository iPatientRepository)
        {
            var doctor = await iStaffRepository.Staff().FirstOrDefaultAsync(x => x.Id.ToString() == request.DoctorId);

            var doctorId = doctor?.Id;

            var patient = await iPatientRepository.Patients()
                                                    .FirstOrDefaultAsync(x => x.Id.ToString() == request.PatientId);


            if (patient == null)
            {
                throw new CustomMessageException("Patient to add to appointmet not found", System.Net.HttpStatusCode.NotFound);
            }

            var appointment = new AppAppointment
            {
                DoctorId = doctorId,
                PatientId = patient?.Id,
                CompanyId = patient?.CompanyId,
                AppointmentDate = request.AppointmentDate.Value
            };
            return appointment;
        }

        public static async Task<AppTicket> CreateNewTicket(CreateTicketCommand request, IAppointmentRepository iAppointmentRepository)
        {
            var existingAppointment = await iAppointmentRepository.AppAppointments()
                                                                        .Include(x => x.Tickets)
                                                                        .FirstOrDefaultAsync(x => x.Id.ToString() == request.AppointmentId);

            if (existingAppointment == null)
            {
                throw new CustomMessageException("Appointment not found", System.Net.HttpStatusCode.NotFound);
            }

            var status = request.AppInventoryType.ParseEnum<AppInventoryType>();

            var existingInventory = existingAppointment.Tickets.FirstOrDefault(x => x.AppInventoryType == status);

            if (existingInventory != null)
            {
                throw new CustomMessageException("Ticket already exixts");
            }

            var newTicket = new AppTicket
            {
                AppointmentId = existingAppointment.Id,
                AppInventoryType = status,
            };
            return newTicket;
        }
    }
}

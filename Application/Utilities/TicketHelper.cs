using Application.Command.AppointmentEntities.AddAppointment;
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
                                                  .Include(x => x.PatientContracts.OrderByDescending(y => y.StartDate).Take(1))
                                                    .ThenInclude(x => x.AppCost)
                                                  .Include(x => x.Company)
                                                    .ThenInclude(x => x.CompanyContracts.OrderByDescending(y => y.StartDate).Take(1))
                                                        .ThenInclude(x => x.AppCost)
                                                  .FirstOrDefaultAsync(x => x.Id.ToString() == request.PatientId);


            if (patient == null)
            {
                throw new CustomMessageException("Patient to add to appointmet not found", System.Net.HttpStatusCode.NotFound);
            }

            if (!patient.HasContract())
            {
                throw new CustomMessageException("Patient does not have any contract at the moment");
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
    }
}

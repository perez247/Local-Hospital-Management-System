using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdateAppointment
{
    public class UpdateAppointmentCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? DoctorId { get; set; }

        [VerifyGuidAnnotation]
        public string? AppointmentId { get; set; }
        public DateTime? AppointmentDate { get; set; }
    }

    public class UpdateAppointmnetHandler : IRequestHandler<UpdateAppointmentCommand, Unit>
    {
        private readonly IPatientRepository iPatientRepository;
        private readonly IStaffRepository iStaffRepository;
        private readonly IDBRepository iDBRepository;

        public UpdateAppointmnetHandler(IPatientRepository IPatientRepository, IDBRepository IDBRepository, IStaffRepository IStaffRepository)
        {
            iPatientRepository = IPatientRepository;
            iDBRepository = IDBRepository;
            iStaffRepository = IStaffRepository;
        }
        public async Task<Unit> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var doctor = await iStaffRepository.Staff()
                                   .FirstOrDefaultAsync(x => x.Id.ToString() == request.DoctorId);

            var doctorId = doctor?.Id;

            var appointment = await iStaffRepository.AppAppointments()
                                                    .FirstOrDefaultAsync(x => x.Id.ToString() == request.AppointmentId);

            if (appointment == null)
            {
                throw new CustomMessageException("Patient to add to appointmet not found", System.Net.HttpStatusCode.NotFound);

            }

            appointment.DoctorId = doctorId;
            appointment.AppointmentDate = request.AppointmentDate.Value;

            iDBRepository.Update<AppAppointment>(appointment);
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}

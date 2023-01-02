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

namespace Application.Command.AddAppointment
{
    public class AppAppointmentCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? DoctorId { get; set; }

        [VerifyGuidAnnotation]
        public string? PatientId { get; set; }
        public DateTime? AppointmentDate { get; set; }
    }

    public class AppAppointmentHandler : IRequestHandler<AppAppointmentCommand, Unit>
    {
        private readonly IPatientRepository iPatientRepository;
        private readonly IStaffRepository iStaffRepository;
        private readonly IDBRepository iDBRepository;

        public AppAppointmentHandler(IPatientRepository IPatientRepository, IDBRepository IDBRepository, IStaffRepository IStaffRepository)
        {
            iPatientRepository = IPatientRepository;
            iDBRepository = IDBRepository;
            iStaffRepository = IStaffRepository;
        }
        public async Task<Unit> Handle(AppAppointmentCommand request, CancellationToken cancellationToken)
        {
            AppAppointment appointment = await TicketHelper.CreateAppointment(request, iStaffRepository, iPatientRepository);

            await iDBRepository.AddAsync<AppAppointment>(appointment);
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}

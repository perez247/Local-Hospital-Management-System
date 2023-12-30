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

namespace Application.Command.AppointmentEntities.AddAppointment
{
    public class AppAppointmentCommand : TokenCredentials, IRequest<AddAppointmentResponse>
    {
        [VerifyGuidAnnotation]
        public string? DoctorId { get; set; }

        [VerifyGuidAnnotation]
        public string? PatientId { get; set; }

        [VerifyGuidAnnotation]
        public string? SponsorId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string? OverallDescription { get; set; }
    }

    public class AppAppointmentHandler : IRequestHandler<AppAppointmentCommand, AddAppointmentResponse>
    {
        private readonly IPatientRepository iPatientRepository;
        private readonly IStaffRepository iStaffRepository;
        private readonly IDBRepository iDBRepository;
        private readonly ICompanyRepository companyRepository;

        public AppAppointmentHandler(IPatientRepository IPatientRepository, IDBRepository IDBRepository, IStaffRepository IStaffRepository, ICompanyRepository ICompanyRepository)
        {
            iPatientRepository = IPatientRepository;
            iDBRepository = IDBRepository;
            iStaffRepository = IStaffRepository;
            companyRepository = ICompanyRepository;
        }
        public async Task<AddAppointmentResponse> Handle(AppAppointmentCommand request, CancellationToken cancellationToken)
        {
            AppAppointment appointment = await TicketHelper.CreateAppointment(request, iStaffRepository, iPatientRepository, companyRepository);

            await iDBRepository.AddAsync(appointment);
            await iDBRepository.Complete();

            return new AddAppointmentResponse { AppointmentId = appointment.Id.ToString() };
        }
    }
}

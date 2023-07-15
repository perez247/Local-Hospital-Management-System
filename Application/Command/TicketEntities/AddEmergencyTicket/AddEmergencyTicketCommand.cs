using Application.Annotations;
using Application.Command.AppointmentEntities.AddAppointment;
using Application.Command.TicketEntities.SaveTicketAndInventory;
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

namespace Application.Command.TicketEntities.AddEmergencyTicket
{
    public class AddEmergencyTicketCommand : TokenCredentials, IRequest<AddEmergencyTicketResponse>
    {
        [VerifyGuidAnnotation]
        public string? PatientId { get; set; }

        [VerifyGuidAnnotation]
        public string? DoctorId { get; set; }

        public string? OverallTicketDescription { get; set; }

        public string? OverallAppointmentDescription { get; set; }

        public string? AppInventoryType { get; set; }

        public ICollection<SaveTicketAndInventoryRequest>? TicketInventories { get; set; }
    }

    public class AddEmergencyTicketHandler : IRequestHandler<AddEmergencyTicketCommand, AddEmergencyTicketResponse>
    {
        private readonly IDBRepository _iDBRepository;
        private readonly IInventoryRepository _iInventoryRepository;
        private readonly IPatientRepository _iPatientRepository;
        private readonly IStaffRepository _iStaffRepository;
        private readonly ITicketRepository _iTicketRepository;
        private readonly IAppointmentRepository _iAppointmentRepository;

        public AddEmergencyTicketHandler(IDBRepository iDBRepository, IInventoryRepository iInventoryRepository, IStaffRepository iStaffRepository, IPatientRepository iPatientRepository, ITicketRepository iTicketRepository, IAppointmentRepository iAppointmentRepository)
        {
            _iDBRepository = iDBRepository;
            _iInventoryRepository = iInventoryRepository;
            _iStaffRepository = iStaffRepository;
            _iPatientRepository = iPatientRepository;
            _iTicketRepository = iTicketRepository;
            _iAppointmentRepository = iAppointmentRepository;
        }

        public async Task<AddEmergencyTicketResponse> Handle(AddEmergencyTicketCommand request, CancellationToken cancellationToken)
        {
            if (request.DoctorId == Guid.Empty.ToString())
            {
                throw new CustomMessageException("Doctor not found");
            }

            // Set the right appointment date
            DateTime appointmentDate = SetAppointmentDate();

            var addApointmentRequest = new AppAppointmentCommand
            {
                DoctorId = request.DoctorId,
                PatientId = request.PatientId,
                AppointmentDate = appointmentDate,
                OverallDescription = request.OverallAppointmentDescription
            };

            // Get appointment
            var appointment = await TicketHelper.CreateAppointment(addApointmentRequest, _iStaffRepository, _iPatientRepository);

            await _iDBRepository.AddAsync<AppAppointment>(appointment);
            // Save appointment to the database
            await _iDBRepository.Complete();

            // Create ticket
            var saveAppointmentTicketCommand = new SaveTicketAndInventoryCommand
            {
                AppointmentId = appointment.Id.ToString(),
                AppInventoryType = request.AppInventoryType,
                OverallDescription = request.OverallTicketDescription,
                TicketInventories = request.TicketInventories,
            };

            var newTicket = await TicketHelper.AddOrUpdateExistingTickets(saveAppointmentTicketCommand, _iTicketRepository, _iInventoryRepository, _iDBRepository, _iAppointmentRepository);

            await _iDBRepository.Complete();

            return new AddEmergencyTicketResponse
            {
                TicketId = newTicket.Id.ToString(),
                AppointmentId = appointment.Id.ToString()
            };
        }

        private static DateTime SetAppointmentDate()
        {
            // Create new appointment

            var appointmentDate = DateTime.Now.AddMinutes(-DateTime.Now.Minute).AddSeconds(-DateTime.Now.Second);

            if (appointmentDate.Hour >= 22)
            {
                appointmentDate = new DateTime(appointmentDate.Year, appointmentDate.Month, appointmentDate.Day, 21, 0, 0);
            }

            if (appointmentDate.Hour <= 2)
            {
                appointmentDate = new DateTime(appointmentDate.Year, appointmentDate.Month, appointmentDate.Day, 3, 0, 0);
            }

            return appointmentDate;
        }
    }
}

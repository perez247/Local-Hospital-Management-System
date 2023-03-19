using Application.Annotations;
using Application.Command.AddAdmissionTicketInventory;
using Application.Command.AddLabTicketInventory;
using Application.Command.AddPharmacyTicketInventory;
using Application.Command.AppointmentEntities.AddAppointment;
using Application.Command.CreateTicket;
using Application.Command.SaveSurgeryTicketInventory;
using Application.Command.TicketEntities.UpdatePharmacyTicketInventory;
using Application.Command.UpdateLabTicketInventory;
using Application.Command.UpdateSurgeryTicketInventory;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.CreateEmergencyAppoitment
{
    public class CreateEmergencyAppointmentCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? DoctorId { get; set; }

        [VerifyGuidAnnotation]
        public string? PatientId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string? AppInventoryType { get; set; }
        public ICollection<AddLabTicketInventoryRequest>? AddLabTicketInventoryRequest { get; set; }
        public ICollection<UpdateLabTicketInventoryRequest>? UpdateLabTicketInventoryRequest { get; set; }
        public ICollection<SaveSurgeyTicketInventoryRequest>? SaveSurgeyTicketInventoryRequest { get; set; }
        public ICollection<UpdateSurgeryTicketInventoryRequest>? UpdateSurgeryTicketInventoryRequest { get; set; }
        public ICollection<AddPharmacyTicketRequest>? TicketInventories { get; set; }
        public ICollection<UpdatePharmacyTicketInventoryRequest>? UpdatePharmacyTicketInventoryRequest { get; set; }
        public AddAdmissionTicketInventoryCommand AddAdmissionRequest { get; set; }
    }

    public class CreateEmergencyAppointmentHandler : IRequestHandler<CreateEmergencyAppointmentCommand, Unit>
    {
        private readonly IPatientRepository iPatientRepository;
        private readonly IStaffRepository iStaffRepository;
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;
        private readonly IAppointmentRepository iAppointmentRepository;

        public CreateEmergencyAppointmentHandler(
            IPatientRepository IPatientRepository, 
            IDBRepository IDBRepository, 
            IStaffRepository IStaffRepository,
            ITicketRepository ITicketRepository,
            IInventoryRepository IInventoryRepository,
            IAppointmentRepository IAppointmentRepository
            )
        {
            iPatientRepository = IPatientRepository;
            iDBRepository = IDBRepository;
            iStaffRepository = IStaffRepository;
            iTicketRepository = ITicketRepository;
            iInventoryRepository = IInventoryRepository;
            iAppointmentRepository = IAppointmentRepository;
        }
        public async Task<Unit> Handle(CreateEmergencyAppointmentCommand request, CancellationToken cancellationToken)
        {
            #region create appointment
            var createAppCommand = new AppAppointmentCommand { DoctorId = request.DoctorId, PatientId = request.PatientId, AppointmentDate = request.AppointmentDate };
            AppAppointment appointment = await TicketHelper.CreateAppointment(createAppCommand, iStaffRepository, iPatientRepository);
            appointment.IsEmergency = true;
            await iDBRepository.AddAsync<AppAppointment>(appointment);

            await iDBRepository.Complete();
            #endregion 

            var type = request.AppInventoryType.ParseEnum<AppInventoryType>();

            #region create ticket

            var newTicketCommand = new CreateTicketCommand { AppointmentId = appointment.Id.ToString(), AppInventoryType = request.AppInventoryType };
            var newTicket = await TicketHelper.CreateNewTicket(newTicketCommand, iAppointmentRepository);

            await iDBRepository.AddAsync<AppTicket>(newTicket);
            await iDBRepository.Complete();
            #endregion

            if (type == AppInventoryType.lab || type == AppInventoryType.radiology)
            {
                var newLabRequest = new AddLabTicketInventoryCommand { TicketId = newTicket.Id.ToString(), AddLabTicketInventoryRequest = request.AddLabTicketInventoryRequest };
                await LabTicketInventoryHelper.AddNewOrExistingLabTickets(newLabRequest, iTicketRepository, iInventoryRepository, iDBRepository);
                
                SendTicketToDepartment(newTicket);

                await iDBRepository.Complete();

                var updateLabRequest = new UpdateLabTicketInventoryCommand { TicketId = newTicket.Id.ToString(), UpdateLabTicketInventoryRequest = request.UpdateLabTicketInventoryRequest };
                await LabTicketInventoryHelper.UpdateLabTickets(updateLabRequest, iTicketRepository, iInventoryRepository, iDBRepository);

                SendTicketToFinanceDepartment(newTicket);
                await iDBRepository.Complete();
            }

            if (type == AppInventoryType.surgery)
            {
                var newSurgeryRequest = new SaveSurgeryTicketInventoryCommand { TicketId = newTicket.Id.ToString(), SaveSurgeyTicketInventoryRequest = request.SaveSurgeyTicketInventoryRequest };
                await SurgeryTicketInventoryHelper.AddNewOrExistingSurgerys(newSurgeryRequest, iTicketRepository, iInventoryRepository, iDBRepository);

                SendTicketToDepartment(newTicket);

                await iDBRepository.Complete();

                var updateSurgeryRequest = new UpdateSurgeryTicketInventoryCommand { TicketId = newTicket.Id.ToString(), UpdateSurgeryTicketInventoryRequest = request.UpdateSurgeryTicketInventoryRequest };
                await SurgeryTicketInventoryHelper.UpdateSurgeryTicketList(updateSurgeryRequest, iTicketRepository, iInventoryRepository, iDBRepository);

                SendTicketToFinanceDepartment(newTicket);
                await iDBRepository.Complete();
            }

            if (type == AppInventoryType.pharmacy)
            {
                var newPharmacyRequest = new AddPharmacyTicketInventoryCommand { TicketId = newTicket.Id.ToString(), TicketInventories = request.TicketInventories };
                await PharmacyTicketInventoryHelper.AddOrUpdateExistingPharmacyTickets(newPharmacyRequest, iTicketRepository, iInventoryRepository, iDBRepository);

                SendTicketToDepartment(newTicket);

                await iDBRepository.Complete();

                var updatePharmacyRequest = new UpdatePharmacyTicketInventoryCommand { TicketId = newTicket.Id.ToString(), UpdatePharmacyTicketInventoryRequest = request.UpdatePharmacyTicketInventoryRequest };
                await PharmacyTicketInventoryHelper.UpdatePharmacyTickets(updatePharmacyRequest, iTicketRepository, iInventoryRepository, iDBRepository);

                SendTicketToFinanceDepartment(newTicket);
                await iDBRepository.Complete();
            }

            if (type == AppInventoryType.admission)
            {
                await AdmissionTickInventoryHelper.AddAdmissionTicket(request.AddAdmissionRequest, iTicketRepository, iInventoryRepository, iDBRepository);
                SendTicketToDepartment(newTicket);
                await iDBRepository.Complete();
            }

            return Unit.Value;
        }

        private void SendTicketToDepartment(AppTicket newTicket)
        {
            newTicket.Sent = true;
            iDBRepository.Update<AppTicket>(newTicket);
        }
        private void SendTicketToFinanceDepartment(AppTicket newTicket)
        {
            newTicket.SentToFinance = true;
            iDBRepository.Update<AppTicket>(newTicket);
        }
    }
}

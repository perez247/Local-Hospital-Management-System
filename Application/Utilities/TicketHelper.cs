using Application.Command.AppointmentEntities.AddAppointment;
using Application.Command.TicketEntities.AddPharmacyTicketInventory;
using Application.Command.TicketEntities.SaveTicketAndInventory;
using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Requests;
using Application.Utilities.QueryHelpers;
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
        public static async Task<AppAppointment> CreateAppointment(AppAppointmentCommand request, IStaffRepository iStaffRepository, IPatientRepository iPatientRepository, ICompanyRepository companyRepository)
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

            // Get home company
            var individualCompany = await CompanyHelper.GetIndividualCompany(companyRepository);
            var companyContract = patient.Company;

            if (individualCompany.Id.ToString() != request.SponsorId && companyContract.Id.ToString() != request.SponsorId)
            {
                throw new CustomMessageException("A sponsor is required");
            }

            var time = request.AppointmentDate.Value.ToUniversalTime().Hour;

            if (time >= 22 || time <= 2)
            {
                throw new CustomMessageException("Hour must be between 3AM and 9PM");
            }

            var appointment = new AppAppointment
            {
                DoctorId = doctorId,
                PatientId = patient?.Id,
                CompanyId = Guid.Parse(request.SponsorId),
                SponsorId = request.SponsorId,
                AppointmentDate = request.AppointmentDate.Value.ToUniversalTime(),
                OverallDescription = string.IsNullOrEmpty(request.OverallDescription) ? null : request.OverallDescription.Trim(),  
            };


            return appointment;
        }

        public static async Task<AppTicket> AddOrUpdateExistingTickets(
            SaveTicketAndInventoryCommand request,
            ITicketRepository iTicketRepository,
            IInventoryRepository iInventoryRepository,
            IDBRepository iDBRepository,
            IAppointmentRepository iAppointmentRepository,
            Boolean fromAdmission = false
            )
        {

            var addmission = request.TicketInventories.FirstOrDefault(x => x.AppInventoryType.ParseEnum<AppInventoryType>() == AppInventoryType.admission);

            if (addmission != null && request.TicketInventories.Count > 1)
            {
                throw new CustomMessageException("You cannot add more prescription with admission. Admission should be added separately.");
            }

            var appointment = await iAppointmentRepository.AppAppointments()
                                                          .Include(x => x.Patient)
                                                          .Include(x => x.Tickets.Where(x => x.Id.ToString() == request.TicketId))
                                                          .FirstOrDefaultAsync(x => x.Id.ToString() == request.AppointmentId);

            if (appointment == null)
            {
                throw new CustomMessageException("Appointment not found");
            }

            var appInventories = request.TicketInventories.Select(x => Guid.Parse(x.InventoryId)).ToList();
            var sponsorId = appointment.CompanyId;

            await InventoryHelper.checkIfCompanyHasInventory(appInventories, sponsorId.Value, iInventoryRepository);

            var ticketFromDb = appointment.Tickets.FirstOrDefault();

            if (ticketFromDb == null)
            {
                ticketFromDb = await CreateNewTicket(request, iTicketRepository, iDBRepository, fromAdmission, appointment, ticketFromDb);
            }
            else
            {
                UpdateExistingTicket(request, iDBRepository, ticketFromDb);
            }

            if (ticketFromDb.AppInventoryType == AppInventoryType.admission && !fromAdmission)
            {
                await ticketFromDb.MustHaveOnlyOneAdmissionRunning(iTicketRepository, appointment.Patient.AppUserId.ToString());
            }

            if (!fromAdmission)
            {
                ticketFromDb.MustNotHaveBeenSentToDepartment();
            }

            request.TicketInventories = request.TicketInventories.DistinctBy(x => x.InventoryId).ToList();

            var ids = request.TicketInventories.Select(x => x.InventoryId);

            if (ids.Count() >= 100)
            {
                throw new CustomMessageException("Only a maximum of 100 inventory per ticket");
            }

            var inventories = await iInventoryRepository.AppInventories()
                                                        .Where(x => ids.Contains(x.Id.ToString()))
                                                        .ToListAsync();

            var ticketInventories = await iTicketRepository.TicketInventory()
                                               .Where(x => x.AppTicketId == ticketFromDb.Id)
                                               .ToListAsync();

            var ticketsInventoriesUpdated = new List<TicketInventory>();

            foreach (var ticketInventory in request.TicketInventories)
            {
                await TicketHelper.SaveTicketInventory(request, ticketInventory, ticketFromDb, inventories, ticketInventories, iDBRepository, ticketsInventoriesUpdated);
            }

            if (!fromAdmission)
            {
                // Delete any ticket that was not found
                var ticketsInventoriesToBeDeleted = ticketInventories.Where(x => !ticketsInventoriesUpdated.Select(a => a.Id.Value).Contains(x.Id.Value));

                if (ticketsInventoriesToBeDeleted.Count() > 0)
                {
                    foreach (var inventory in ticketsInventoriesToBeDeleted)
                    {
                        iDBRepository.Remove<TicketInventory>(inventory);
                    }
                }
            }

            var ticket = new AppTicket
            {
                Id = ticketFromDb.Id,
                TicketInventories = ticketsInventoriesUpdated
            };

            return ticket;
        }

        private static void UpdateExistingTicket(SaveTicketAndInventoryCommand request, IDBRepository iDBRepository, AppTicket? ticketFromDb)
        {
            ticketFromDb.OverallDescription = request.OverallDescription.Trim();
            iDBRepository.Update<AppTicket>(ticketFromDb);
        }

        private static async Task<AppTicket?> CreateNewTicket(SaveTicketAndInventoryCommand request, ITicketRepository iTicketRepository, IDBRepository iDBRepository, bool fromAdmission, AppAppointment? appointment, AppTicket? ticketFromDb)
        {
            var totalAppointmentTickets = await iTicketRepository.AppTickets().CountAsync(x => x.AppointmentId.ToString() == request.AppointmentId);

            if (!fromAdmission)
            {
                if (totalAppointmentTickets >= 20)
                {
                    throw new CustomMessageException("Only a maximum of 20 tickets per appointment");
                }

                if (request.AppInventoryType.ParseEnum<AppInventoryType>() == AppInventoryType.admission)
                {
                    var totalAdmissionTicket = await iTicketRepository.AppTickets().CountAsync(x => x.AppointmentId.ToString() == request.AppointmentId && x.AppInventoryType == AppInventoryType.admission);

                    if (totalAdmissionTicket > 0)
                    {
                        throw new CustomMessageException("This appointment has already prescribed an admission for the patient");
                    }
                }
            }

            ticketFromDb = new AppTicket
            {
                Id = Guid.NewGuid(),
                AppInventoryType = request.AppInventoryType.ParseEnum<AppInventoryType>(),

                AppointmentId = request.AppointmentId != Guid.Empty.ToString() ? Guid.Parse(request.AppointmentId) : null,
            };

            if (appointment != null)
            {
                ticketFromDb.DateCreated = appointment.AppointmentDate >= DateTime.Now ? appointment.AppointmentDate.AddMinutes(5).ToUniversalTime() : DateTime.Now;
            }

            ticketFromDb.OverallDescription = request.OverallDescription.Trim();
            await iDBRepository.AddAsync<AppTicket>(ticketFromDb);
            return ticketFromDb;
        }

        private static async Task SaveTicketInventory(
            SaveTicketAndInventoryCommand command,
            SaveTicketAndInventoryRequest request,
            AppTicket? ticketFromDb,
            List<AppInventory> appInventories,
            List<TicketInventory> ticketInventories,
            IDBRepository iDBRepository,
            ICollection<TicketInventory> ticketsInventoriesUpdated
            )
        {
            var inventory = appInventories.FirstOrDefault(x => x.Id.ToString() == request.InventoryId);

            if (inventory == null)
            {
                throw new CustomMessageException($"{command.AppInventoryType} to add not found");
            }

            var hasInventory = ticketInventories.FirstOrDefault(x => x.Id.ToString() == request.TicketInventoryId);
            if (hasInventory != null)
            {
                hasInventory.DoctorsPrescription = string.IsNullOrEmpty(request.DoctorsPrescription) ? null : request.DoctorsPrescription;
                hasInventory.Times = request.Times;
                hasInventory.Dosage = request.Dosage;
                hasInventory.Frequency = request.Frequency;
                hasInventory.Duration = request.Duration;
                hasInventory.StaffObservation = request.StaffObservation;
                hasInventory.AppInventory = inventory;

                hasInventory.PrescribedQuantity = (request.Times * request.Dosage * request.Duration).ToString();

                iDBRepository.Update<TicketInventory>(hasInventory);
            }
            else
            {
                hasInventory = new TicketInventory
                {
                    Id = Guid.NewGuid(),
                    AppInventoryId = inventory.Id,
                    AppInventory = inventory,
                    AppTicketId = ticketFromDb.Id,
                    DoctorsPrescription = string.IsNullOrEmpty(request.DoctorsPrescription) ? null : request.DoctorsPrescription,
                    PrescribedQuantity = (request.Times * request.Dosage * request.Duration).ToString(),
                    Times = request.Times,
                    Dosage = request.Dosage,
                    Frequency = request.Frequency,
                    Duration = request.Duration,
                    StaffObservation = request.StaffObservation,
                    StaffId = command.getCurrentUserRequest().CurrentUser.Staff.Id,
                };

                await iDBRepository.AddAsync<TicketInventory>(hasInventory);
            }

            ticketsInventoriesUpdated.Add(hasInventory);
        }

        public static async Task VerifyInventories(
            ICollection<SendTicketToFinanceRequest> requestTicketInventories,
            AppTicket? ticketFromDb,
            IEnumerable<string?> inventoryIds,
            ITicketRepository iTicketRepository,
            IInventoryRepository iInventoryRepository,
            IDBRepository iDBRepository,
            Func<SendTicketToFinanceRequest?, TicketInventory?, AppInventory?, object>? moreValidation
            )
        {
            var inventories = await iInventoryRepository.AppInventories()
                                            .Where(x => inventoryIds.Contains(x.Id.ToString()))
                                            .ToListAsync();

            var ticketInventories = await iTicketRepository.TicketInventory()
                                                           .Where(x => x.AppTicketId == ticketFromDb.Id)
                                                           .ToListAsync();

            foreach (var itemTicket in requestTicketInventories)
            {
                var inventory = inventories.FirstOrDefault(x => x.Id.ToString() == itemTicket.InventoryId);

                if (inventory == null)
                {
                    throw new CustomMessageException("Item not recorded in the inventory");
                }

                var pharmacyTicketInventory = ticketInventories.FirstOrDefault(x => x.Id.ToString() == itemTicket.TicketInventoryId);

                if (pharmacyTicketInventory == null)
                {
                    throw new CustomMessageException("Item not found for this ticket");
                }

                if (moreValidation != null)
                {
                    moreValidation(itemTicket, pharmacyTicketInventory, inventory);
                }

                pharmacyTicketInventory.PrescribedQuantity = itemTicket.PrescribedQuantity.ToString();
                pharmacyTicketInventory.DepartmentDescription = itemTicket.DepartmentDescription;
                pharmacyTicketInventory.AppTicketStatus = itemTicket.AppTicketStatus.ParseEnum<AppTicketStatus>();

                iDBRepository.Update(pharmacyTicketInventory);
            }
        }

        public static async Task<AppTicket?> BasicConclusion<T>(
            string ticketId, 
            ICollection<T>? ConcludeTicketRequest,
            ITicketRepository iTicketRepository,
            IDBRepository iDBRepository,
            Boolean trackForSaving = true
            ) where T : ConcludeTicketRequest
        {
            var ticketFromDb = await iTicketRepository.AppTickets()
                                          .Include(x => x.TicketInventories)
                                            .ThenInclude(x => x.AppInventory)
                                          .Include(x => x.TicketInventories)
                                            .ThenInclude(x => x.SurgeryTicketPersonnels)
                                          .Include(x => x.AppCost)
                                          .FirstOrDefaultAsync(x => x.Id.ToString() == ticketId);

            ticketFromDb.MustHaveBeenSentToFinance();

            if (ticketFromDb.AppTicketStatus == AppTicketStatus.concluded)
            {
                throw new CustomMessageException("Ticket has already been concluded");
            }

            var appCost = ticketFromDb.AppCost;
            if (appCost.PaymentStatus == PaymentStatus.pending)
            {
                throw new CustomMessageException("Payment status should not be pending");
            }

            ConcludeTicketRequest = ConcludeTicketRequest.DistinctBy(x => x.InventoryId).ToList();


            var ticketInventories = ticketFromDb.TicketInventories;

            foreach (var genericTicketInventory in ConcludeTicketRequest)
            {

                var ticketInventory = ticketInventories.FirstOrDefault(x => x.Id.ToString() == genericTicketInventory.InventoryId);

                if (ticketInventory == null)
                {
                    throw new CustomMessageException("Item not found for this ticket");
                }

                if (ticketInventory.AppInventory == null)
                {
                    throw new CustomMessageException("Item not found in the inventory");
                }

                ticketInventory.ConcludedDate = genericTicketInventory.ConcludedDate;
                ticketInventory.AppTicketStatus = genericTicketInventory.AppTicketStatus.ParseEnum<AppTicketStatus>();
                ticketInventory.Proof = genericTicketInventory.Proof;

                if (trackForSaving)
                {
                    iDBRepository.Update(ticketInventory);
                }

            }

            ticketFromDb.AppTicketStatus = AppTicketStatus.concluded;

            return ticketFromDb;
        }

        public static async Task<AppTicket?> BasicConclusion2(
        string ticketId,
        ITicketRepository iTicketRepository,
        IDBRepository iDBRepository
        )
        {
            var ticketFromDb = await iTicketRepository.AppTickets()
                                          .Include(x => x.TicketInventories)
                                            .ThenInclude(x => x.AppInventory)
                                          .Include(x => x.TicketInventories)
                                            .ThenInclude(x => x.SurgeryTicketPersonnels)
                                          .Include(x => x.AppCost)
                                          .FirstOrDefaultAsync(x => x.Id.ToString() == ticketId);

            ticketFromDb.MustHaveBeenSentToFinance();

            if (ticketFromDb.AppTicketStatus == AppTicketStatus.concluded)
            {
                throw new CustomMessageException("Ticket has already been concluded");
            }

            var appCost = ticketFromDb.AppCost;

            if (appCost == null)
            {
                throw new CustomMessageException("Client has not been billed yet");
            }

            if (appCost.PaymentStatus == PaymentStatus.pending)
            {
                throw new CustomMessageException("Payment status should not be pending");
            }

            var ticketInventories = ticketFromDb.TicketInventories.Count(x => x.AppTicketStatus == AppTicketStatus.ongoing || x.AppTicketStatus == AppTicketStatus.concluded);

            if (ticketInventories == 0)
            {
                throw new CustomMessageException("Ar least one ongoing/concluded ticket inventory must be present");
            }

            foreach (var ticketInventory in ticketFromDb.TicketInventories)
            {

                if (ticketInventory.AppInventory == null)
                {
                    throw new CustomMessageException("Item not found in the inventory");
                }

                if (ticketInventory.AppTicketStatus == AppTicketStatus.concluded)
                {
                    throw new CustomMessageException("Some inventories have already been concluded");
                }

                if (!ticketInventory.ConcludedDate.HasValue)
                {
                    ticketInventory.ConcludedDate = DateTime.Now.ToUniversalTime();

                    if (ticketInventory.AppTicketStatus == AppTicketStatus.ongoing)
                    {
                        ticketInventory.AppTicketStatus = AppTicketStatus.concluded;
                    }
                    iDBRepository.Update(ticketInventory);
                }
            }

            ticketFromDb.AppTicketStatus = AppTicketStatus.concluded;
            iDBRepository.Update<AppTicket>(ticketFromDb);

            return ticketFromDb;
        }




        public static void MustNotHaveBeenSentToDepartment(this AppTicket? ticketFromDb)
        {
            if (ticketFromDb == null)
            {
                throw new CustomMessageException("Ticket not found");
            }

            if (ticketFromDb.Sent.HasValue && ticketFromDb.Sent.Value)
            {
                throw new CustomMessageException("Ticket has already been sent to a department");
            }
        }

        public static void CancelIfSentToDepartmentAndFinance(this AppTicket? ticketFromDb)
        {
            if (ticketFromDb == null)
            {
                throw new CustomMessageException("Ticket not found");
            }

            if ((ticketFromDb.Sent.HasValue && ticketFromDb.Sent.Value) && (ticketFromDb.SentToFinance.HasValue && ticketFromDb.SentToFinance.Value))
            {
                throw new CustomMessageException("Ticket has already been sent to all departments");
            }
        }
        public static void MustHaveBeenSentToFinance(this AppTicket? ticketFromDb)
        {
            if (ticketFromDb == null)
            {
                throw new CustomMessageException("Ticket not found");
            }

            if (!ticketFromDb.SentToFinance.HasValue)
            {
                throw new CustomMessageException("Ticket must be sent to the finance department");
            }

            if (!ticketFromDb.SentToFinance.Value)
            {
                throw new CustomMessageException("Ticket must be sent to the finance department");
            }
        }

        public static void MustHvaeBeenSentToDepartment(this AppTicket? ticketFromDb)
        {
            if (ticketFromDb == null)
            {
                throw new CustomMessageException("Ticket not found");
            }

            if (!ticketFromDb.Sent.HasValue)
            {
                throw new CustomMessageException("Ticket must be sent to the department");
            }

            if (!ticketFromDb.Sent.Value)
            {
                throw new CustomMessageException("Ticket must be sent to the department");
            }
        }

        public static async Task MustHaveOnlyOneAdmissionRunning(this AppTicket? ticketFromDb, ITicketRepository iTicketRepository, string userId)
        {
            var admission = await iTicketRepository.AppTickets()
                                             .Include(x => x.Appointment)
                                                .ThenInclude(x => x.Patient)
                                             .OrderByDescending(x => x.DateCreated)
                                             .Where(x =>
                                                x.AppInventoryType == AppInventoryType.admission &&
                                                x.Appointment.Patient.AppUserId.ToString() == userId &&
                                                x.AppTicketStatus == AppTicketStatus.ongoing &&
                                                x.SentToFinance.HasValue &&
                                                x.SentToFinance.Value
                                                )
                                             .Take(1)
                                             .FirstOrDefaultAsync();

            if (admission != null)
            {
                throw new CustomMessageException("Patient has already been admitted. kindly admission before creating a new one");
            }
        }
    }
}

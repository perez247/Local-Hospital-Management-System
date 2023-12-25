using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.TicketEntities.GetAdmissionStats
{
    public class GetAdmissionStatsQuery : TokenCredentials, IRequest<GetAdmissionStatsResponse>
    {
        [VerifyGuidAnnotation]
        public string? TicketId { get; set; }
    }

    public class GetAdmissionStats : IRequestHandler<GetAdmissionStatsQuery, GetAdmissionStatsResponse>
    {
        private readonly ITicketRepository iTicketRepository;

        public GetAdmissionStats(ITicketRepository ITicketRepository)
        {
            iTicketRepository = ITicketRepository;
        }

        public async Task<GetAdmissionStatsResponse> Handle(GetAdmissionStatsQuery request, CancellationToken cancellationToken)
        {
            var stats = await iTicketRepository.AppTickets()
                                               .Include(x => x.AppCost)
                                               .Include(x => x.TicketInventories)
                                                .ThenInclude(x => x.AppInventory)
                                               //.Include(x => x.TicketInventories)
                                               // .ThenInclude(x => x.AdmissionPrescription)
                                               .Include(x => x.AdmissionPrescriptions)
                                               .Include(x => x.Appointment)
                                                .ThenInclude(x => x.Patient)
                                                    .ThenInclude(x => x.AppUser)
                                               .Include(x => x.Appointment)
                                                .ThenInclude(x => x.Patient)
                                                    .ThenInclude(x => x.Company)
                                                        .ThenInclude(x => x.AppUser)
                                                .Select(x => new GetAdmissionStatsDTO
                                                {
                                                    AppTicketId = x.Id,
                                                    AppTicket = x,
                                                    Patient = x.Appointment.Patient,
                                                    //TicketInventories = x.TicketInventories,
                                                    TicketInventories = x.TicketInventories.Where(x => x.AppInventory.AppInventoryType == Models.Enums.AppInventoryType.admission).ToList(),
                                                    Pharmacy = x.AdmissionPrescriptions.Count(a => a.AppInventoryType == Models.Enums.AppInventoryType.pharmacy),
                                                    Lab = x.AdmissionPrescriptions.Count(a => a.AppInventoryType == Models.Enums.AppInventoryType.lab),
                                                    Radiology = x.AdmissionPrescriptions.Count(a => a.AppInventoryType == Models.Enums.AppInventoryType.radiology),
                                                    Surgery = x.AdmissionPrescriptions.Count(a => a.AppInventoryType == Models.Enums.AppInventoryType.surgery),
                                                })
                                                .FirstOrDefaultAsync(x => x.AppTicketId.ToString() == request.TicketId);

            if (stats == null)
            {
                throw new CustomMessageException("Admission not found", System.Net.HttpStatusCode.NotFound);
            }

            return GetAdmissionStatsResponse.Create(stats);
        }
    }
}

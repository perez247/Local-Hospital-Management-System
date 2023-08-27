using Application.Responses;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.TicketEntities.GetAdmissionStats
{
    public class GetAdmissionStatsResponse
    {
        public string? AppTicketId { get; set; }
        public PatientResponse? Patient { get; set; }
        public int? Pharmacy { get; set; }
        public int? Lab { get; set; }
        public int? Radiology { get; set; }
        public int? Surgery { get; set; }
        public IEnumerable<TicketInventoryResponse>? TicketInventories { get; set; }

        public static GetAdmissionStatsResponse? Create(GetAdmissionStatsDTO GetAdmissionStatsDTO)
        {
            if (GetAdmissionStatsDTO == null)
            {
                return null;
            }

            return new GetAdmissionStatsResponse
            {
                AppTicketId = GetAdmissionStatsDTO.AppTicketId.ToString(),
                Patient = PatientResponse.Create(GetAdmissionStatsDTO.Patient),
                TicketInventories = GetAdmissionStatsDTO.TicketInventories != null && GetAdmissionStatsDTO.TicketInventories.Count > 0 ?
                                    GetAdmissionStatsDTO.TicketInventories.Select(x => TicketInventoryResponse.Create(x)) : null,
                Pharmacy = GetAdmissionStatsDTO.Pharmacy,
                Lab = GetAdmissionStatsDTO.Lab,
                Radiology = GetAdmissionStatsDTO.Radiology,
                Surgery = GetAdmissionStatsDTO?.Surgery,
            };
        }
    }
}

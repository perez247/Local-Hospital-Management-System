using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities
{
    public static class InventoryHelper
    {
        public static void ValidateInventoryConclusion(TicketInventory? item)
        {
            if (item.AppInventory.AppInventoryType == Models.Enums.AppInventoryType.lab)
            {
                if (string.IsNullOrEmpty(item.LabRadiologyTestResult))
                {
                    throw new CustomMessageException($"Lab - \"{item.AppInventory.Name}\" requires a result");
                }
            }

            if (item.AppInventory.AppInventoryType == Models.Enums.AppInventoryType.radiology)
            {
                if (string.IsNullOrEmpty(item.LabRadiologyTestResult))
                {
                    throw new CustomMessageException($"Radiology - \"{item.AppInventory.Name}\" requires a result");
                }
            }

            if (item.AppInventory.AppInventoryType == Models.Enums.AppInventoryType.surgery)
            {
                if (string.IsNullOrEmpty(item.SurgeryTestResult))
                {
                    throw new CustomMessageException($"Surgery - \"{item.AppInventory.Name}\" requires a result");
                }

                if (item.SurgeryTicketPersonnels.Count == 0)
                {
                    throw new CustomMessageException($"Surgery - \"{item.AppInventory.Name}\" requires at least one personnel");
                }
            }

            if (item.AppInventory.AppInventoryType == Models.Enums.AppInventoryType.admission)
            {
                if (!item.AdmissionStartDate.HasValue)
                {
                    throw new CustomMessageException($"Admission - \"{item.AppInventory.Name}\" requires a start date");
                }

                if (!item.AdmissionStartDate.HasValue || !item.AdmissionEndDate.HasValue)
                {
                    throw new CustomMessageException($"Admission - \"{item.AppInventory.Name}\" requires an end date");
                }

                var totalDays = item.AdmissionEndDate - item.AdmissionStartDate;

                if (totalDays.Value.TotalDays <= 0)
                {
                    throw new CustomMessageException($"Admission - \"{item.AppInventory.Name}\" Duration must be greater than zero");
                }
            }
        }

        public static async Task checkIfCompanyHasInventory(ICollection<Guid> appInventoryIds, Guid companyId, IInventoryRepository iInventoryRepository)
        {
            var appInventories = iInventoryRepository.AppInventories()
                                                    .Include(x => x.AppInventoryItems.Where(a => a.CompanyId == companyId))
                                                   .Where(x => appInventoryIds.Contains(x.Id.Value));

            foreach (var item in appInventories)
            {
                if (item.AppInventoryItems.FirstOrDefault() == null)
                {
                    throw new CustomMessageException($"{item.Name} is not supported by the company sponsoring this patient");
                }
            }
        }
    }
}

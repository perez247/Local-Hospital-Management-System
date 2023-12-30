using Application.Interfaces.IRepositories;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities
{
    public static class ActivityLogHelper
    {
        public static void LogInventoryQuantity(ActivityLog log, IDBRepository dBRepository, AppInventory appInventory, int oldQuantity)
        {
            log.ActionDescription = $"Update Quantity from {oldQuantity} to {appInventory.Quantity}";

            dBRepository.Update<ActivityLog>(log);
        }
    }
}

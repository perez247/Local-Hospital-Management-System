using Application.Command.AdmissionEntities.ExecutePrescription;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities.QueryHelpers
{
    public static class FinancialHelper
    {
        public static FinancialRecord FinancialRecordFactory(Guid? payerId, Guid? payeeId, decimal cost, string description, AppCostType costType)
        {
            return new FinancialRecord
            {
                Id = Guid.NewGuid(),
                Amount = cost,
                ApprovedAmount = cost,
                CostType = costType,
                Description = description,
                FinancialRecordPayerPayees = new List<FinancialRecordPayerPayee>()
                {
                    new FinancialRecordPayerPayee{ AppUserId = payeeId },
                    new FinancialRecordPayerPayee{ AppUserId = payerId, Payer = true },
                }
            };
        }

        public static AppCost AppCostFactory(Guid? payerId, Guid? payeeId, decimal cost, string Description, AppCostType costType)
        {
            var Id = Guid.NewGuid();
            return new AppCost
            {
                Id = Id,
                Amount = cost,
                ApprovedPrice = cost,
                CostType = costType,
                Description = Description,
                FinancialRecordPayerPayees = new List<FinancialRecordPayerPayee>()
                {
                    new FinancialRecordPayerPayee{ AppUserId = payeeId, AppCostId = Id },
                    new FinancialRecordPayerPayee{ AppUserId = payerId, Payer = true, AppCostId = Id },
                }
            };
        }

        public static FinancialRecord AppCostToFinancialRecord(AppCost? appCost)
        {
            var Id = Guid.NewGuid();
            return new FinancialRecord
            {
                Id = Id,
                Amount = appCost.Amount,
                ApprovedAmount = appCost.ApprovedPrice,
                CostType = appCost.CostType,
                Description = appCost.Description,
                FinancialRecordPayerPayees = appCost.FinancialRecordPayerPayees.Select(x => new FinancialRecordPayerPayee
                {
                    AppUserId = x.AppUserId,
                    Payer = x.Payer,
                    FinancialRecordId = Id
                }).ToList(),
            };
        }

        public static async Task UpdateQuantity(
            TicketInventory ticketInventory, 
            AppInventory? appInventory, 
            int newQuantity,
            Guid? userId,
            IDBRepository dBRepository,
            string actionName
            )
        {
            var oldInventoryQuantity = appInventory.Quantity;
            var oldQuantity = Int32.Parse(ticketInventory.PrescribedQuantity);

            if (ticketInventory.LoggedQuantity.HasValue && ticketInventory.LoggedQuantity.Value && oldQuantity != newQuantity)
            {
                appInventory.Quantity += oldQuantity;
            }
            else
            {
                ticketInventory.LoggedQuantity = true;
            }

            ticketInventory.PrescribedQuantity = newQuantity.ToString();

            if (appInventory.Quantity < newQuantity)
            {
                throw new CustomMessageException($"Quantity of {ticketInventory.AppInventory.Name} is insufiicient");
            }

            appInventory.Quantity -= newQuantity;

            if (appInventory.Quantity != oldInventoryQuantity)
            {
                var duringEvent = actionName == nameof(ExecutePrescriptionCommand) ? "during nursing" : "during Billing";
                var newActivityLog = new ActivityLog
                {
                    ActorId = userId,
                    ActionType = actionName,
                    ObjectType = nameof(AppInventory),
                    ObjectId = appInventory.Id.ToString(),
                    ActionDescription = $"Update Quantity from {oldInventoryQuantity} to {appInventory.Quantity} by subtracting {newQuantity} {duringEvent}",
                    OtherDescription = "",
                };

                dBRepository.Update<AppInventory>(appInventory);
                await dBRepository.AddAsync<ActivityLog>(newActivityLog);

            }
        }
    }
}

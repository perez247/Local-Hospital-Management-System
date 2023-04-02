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
    }
}

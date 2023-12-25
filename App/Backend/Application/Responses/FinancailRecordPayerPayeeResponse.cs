using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class FinancailRecordPayerPayeeResponse
    {
        public UserOnlyResponse? AppUser { get; set; }
        public bool Payer { get; set; }

        public static FinancailRecordPayerPayeeResponse? Create(FinancialRecordPayerPayee financialRecordPayerPayee)
        {
            if (financialRecordPayerPayee == null)
            {
                return null;
            }

            return new FinancailRecordPayerPayeeResponse
            {
                Payer = financialRecordPayerPayee.Payer,
                AppUser = UserOnlyResponse.Create(financialRecordPayerPayee.AppUser),
            };
        }
    }
}

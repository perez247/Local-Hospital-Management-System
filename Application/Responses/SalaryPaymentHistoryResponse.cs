using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class SalaryPaymentHistoryResponse
    {
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public decimal Savings { get; set; }
        public bool Paid { get; set; }
        public string? StaffId { get; set; }
        public DateTime DatePaidFor { get; set; }
        public UserResponse? User { get; set; }
        public BaseResponse? Base { get; set; }

        public static SalaryPaymentHistoryResponse Create(SalaryPaymentHistory SalaryPaymentHistory) 
        {
            var data = new SalaryPaymentHistoryResponse();
            data.Amount = SalaryPaymentHistory.Amount;
            data.Tax = SalaryPaymentHistory.Tax;
            data.Savings = SalaryPaymentHistory.Savings;
            data.Paid = SalaryPaymentHistory.Paid;
            data.DatePaidFor = SalaryPaymentHistory.DatePaidFor;
            data.StaffId = SalaryPaymentHistory.StaffId.ToString();
            data.Base = BaseResponse.Create(SalaryPaymentHistory);
            data.User = UserResponse.Create(SalaryPaymentHistory.Staff.AppUser);

            return data;
        }
    }
}

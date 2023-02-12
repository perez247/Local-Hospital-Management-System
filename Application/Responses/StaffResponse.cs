using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class StaffResponse
    {
        public string? Level { get; set; }
        public string? ContractStaff { get; set; }
        public decimal? Salary { get; set; }
        public string? Position { get; set; }
        public bool Active { get; set; } = false;
        public DateTime LastSavingPaymentDate { get; set; }
        public DateTime NextSavingPaymentDate { get; set; }
        public string? UserId { get; set; }
        public BaseResponse? Base { get; set; }
        public DateTime? LastSalaryPayment { get; set; }
        public string? AccountNumber { get; set; }
        public string? BankName { get; set; }
        public string? BankId { get; set; }
        public UserOnlyResponse? User { get; set; }
        public static StaffResponse? Create(Staff staff)
        {
            if (staff == null)
            {
                return null;
            }

            var data = new StaffResponse();
            data.Level = staff.Level;
            data.ContractStaff = staff.ContractStaff.ToString();
            data.Position = staff.Position;
            data.Active = staff.Active;
            data.Salary = staff.Salary;
            data.LastSavingPaymentDate = staff.LastSavingPaymentDate;
            data.NextSavingPaymentDate = staff.NextSavingPaymentDate;
            data.UserId = staff.AppUserId?.ToString();
            data.AccountNumber = staff.AccountNumber;
            data.BankName = staff.BankName;
            data.BankId = staff.BankId;
            data.Base = BaseResponse.Create(staff);
            data.User = UserOnlyResponse.Create(staff.AppUser);

            return data;
        }
    }
}

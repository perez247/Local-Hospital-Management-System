using Application.Responses;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.ViewStaff
{
    public class ViewStaffResponse
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
        public static ViewStaffResponse? Create(Staff staff)
        {
            if (staff == null)
            {
                return null;
            }

            var data = new ViewStaffResponse();
            data.Level = staff.Level;
            data.ContractStaff = staff.ContractStaff.ToString();
            data.Position = staff.Position;
            data.Active = staff.Active;
            data.Salary = staff.Salary;
            data.LastSavingPaymentDate = staff.LastSavingPaymentDate;
            data.NextSavingPaymentDate = staff.NextSavingPaymentDate;
            data.UserId = staff.AppUserId?.ToString();
            data.Base = BaseResponse.Create(staff);

            return data;
        }
    }
}

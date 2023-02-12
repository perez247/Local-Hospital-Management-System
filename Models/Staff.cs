using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Staff : BaseEntity
    {
        public virtual AppUser? AppUser { get; set; }
        public Guid? AppUserId { get; set; }
        public string? Level { get; set; } = "L1";
        public ContractTypeEnum ContractStaff { get; set; } = ContractTypeEnum.contract;
        public decimal? Salary { get; set; } = 15000;
        public string? Position { get; set; } = "Cleaner";
        public bool Active { get; set; } = false;
        public string? AccountNumber { get; set; } = "0000000000";
        public string? BankName { get; set; } = "unknown";
        public string? BankId { get; set; } = "unknwon";
        public DateTime LastSavingPaymentDate { get; set; } = DateTime.UtcNow;
        public DateTime NextSavingPaymentDate { get; set; } = DateTime.UtcNow.AddMonths(6);
        public ICollection<SalaryPaymentHistory>? SalaryPaymentHistory { get; set; } = new List<SalaryPaymentHistory>();
        public ICollection<StaffContract>? StaffContract { get; set; } = new List<StaffContract>();
        public ICollection<StaffTimeTable>? TimeTable { get; set; } = new List<StaffTimeTable>();
        public ICollection<AppCost>? AppCosts { get; set; } = new List<AppCost>();
        public ICollection<PatientVital> PatientVitals { get; set; } = new List<PatientVital>();
        public ICollection<AppAppointment> AppAppointments { get; set; } = new List<AppAppointment>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Patient : BaseEntity
    {
        public virtual AppUser? AppUser { get; set; }
        public Guid? AppUserId { get; set; }
        public virtual Company? Company { get; set; }
        public Guid? CompanyId { get; set; }
        public string? Allergies { get; set; }
        public ICollection<PatientContract> PatientContracts { get; set; } = new List<PatientContract>();
        public ICollection<PatientVital> PatientVitals { get; set; } = new List<PatientVital>();
        public ICollection<AppAppointment> AppAppointments { get; set; } = new List<AppAppointment>();
        public ICollection<AppTicket> AppTickets { get; set; } = new List<AppTicket>();

        public bool HasContract()
        {
            var companyContract = Company?.CompanyContracts.FirstOrDefault();
            BaseContract? contract;
            bool IsPatient = false;

            if (companyContract != null && !Company.ForIndividual)
            {
                contract = companyContract;
            } else
            {
                IsPatient = true;
                var patientContract = PatientContracts.FirstOrDefault();

                if (patientContract != null)
                {
                    contract = patientContract;
                } else
                {
                    contract = null;
                }
            }

            if (contract == null)
            {
                return false;
            }

            if (contract.AppCost == null)
            {
                return false;
            }

            if (contract.AppCost.PaymentStatus == Enums.PaymentStatus.canceled)
            {
                return false;
            }

            if (IsPatient && contract.AppCost.PaymentStatus == Enums.PaymentStatus.owing)
            {
                return false;
            }

            if (contract.AppCost.PaymentStatus == Enums.PaymentStatus.owing || contract.AppCost.PaymentStatus == Enums.PaymentStatus.approved)
            {
                return contract.StartDate.AddDays(contract.Duration) > DateTime.Now;
            }

            return false;
        }
    }
}

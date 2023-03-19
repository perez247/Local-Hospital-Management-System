using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Company : BaseEntity
    {
        public virtual AppUser? AppUser { get; set; }
        public Guid? AppUserId { get; set; }
        public string? Description { get; set; }
        public string? UniqueId { get; set; }
        public string? OtherId { get; set; }
        public bool ForIndividual { get; set; } = false;
        public bool HomeCompany { get; set; } = false;
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();
        public ICollection<CompanyContract> CompanyContracts { get; set; } = new List<CompanyContract>();
        public ICollection<AppInventoryItem>? AppInventoryItems { get; set; } = new List<AppInventoryItem>();
        public ICollection<AppAppointment> AppAppointments { get; set; } = new List<AppAppointment>();
        public ICollection<AppTicket> AppTickets { get; set; } = new List<AppTicket>();
    }
}

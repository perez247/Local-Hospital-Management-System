using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AppAppointment : BaseEntity
    {
        public virtual Staff? Doctor { get; set; }
        public Guid? DoctorId { get; set; }
        public virtual Patient? Patient { get; set; }
        public Guid? PatientId { get; set; }
        public virtual Company? Company { get; set; }
        public Guid? CompanyId { get; set; }
        public bool? IsEmergency { get; set; } = false;
        public DateTime AppointmentDate { get; set; }
        public string? OverallDescription { get; set; }
        public string? SponsorId { get; set; }
        public ICollection<AppTicket> Tickets { get; set; } = new List<AppTicket>();
    }
}

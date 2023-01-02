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
    }
}

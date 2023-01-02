using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PatientVital : BaseEntity
    {
        public virtual Staff? Nurse { get; set; }
        public Guid? NurseId { get; set; }
        public virtual Patient? Patient { get; set; }
        public Guid? PatientId { get; set; }
        public string? Data { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PatientContract : BaseEntity
    {
        public virtual Patient? Patient { get; set; }
        public Guid? PatientId { get; set; }
        public virtual AppCost? AppCost { get; set; }
        public Guid? AppCostId { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
    }
}

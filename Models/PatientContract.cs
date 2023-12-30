using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PatientContract : BaseContract
    {
        public virtual Patient? Patient { get; set; }
        public Guid? PatientId { get; set; }
    }
}

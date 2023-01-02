using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class StaffTimeTable : BaseEntity
    {
        public virtual Staff? Staff { get; set; }
        public Guid? StaffId { get; set; }
        public DateTime ClockIn { get; set; }
        public DateTime ClockOut { get; set; }
        public DateTime? StaffClockIn { get; set; }
        public DateTime? StaffClockOut { get; set; }
    }
}

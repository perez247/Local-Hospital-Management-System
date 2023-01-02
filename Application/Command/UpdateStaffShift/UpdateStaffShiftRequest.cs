using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdateStaffShift
{
    public class UpdateStaffShiftRequest
    {
        [VerifyGuidAnnotation]
        public string? StaffId { get; set; }
        public DateTime? ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }
    }
}

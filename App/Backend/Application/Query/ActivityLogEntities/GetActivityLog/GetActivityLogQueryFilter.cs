using Application.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.ActivityLogEntities.GetActivityLog
{
    public class GetActivityLogQueryFilter
    {
        [VerifyGuidAnnotation]
        public string? ActorId { get; set; }

        [VerifyGuidAnnotation]
        public string? ActeeId { get; set; }
        public string? ObjectType { get; set; }
        public string? ObjectId { get; set; }
        public string? ActionType { get; set; }
    }
}

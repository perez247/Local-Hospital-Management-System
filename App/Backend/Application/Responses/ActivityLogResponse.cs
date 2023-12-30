using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class ActivityLogResponse
    {
        public UserResponse? Actor { get; set; }
        public UserResponse? Actee { get; set; }
        public string? ObjectType { get; set; }
        public string? ObjectId { get; set; }
        public string? ActionType { get; set; }
        public string? ActionDescription { get; set; }
        public string? OtherDescription { get; set; }
        public BaseResponse? Base { get; set; }

        public static ActivityLogResponse? Create(ActivityLog activityLog)
        {
            if (activityLog == null)
            {
                return null;
            }

            return new ActivityLogResponse
            {
                Actor = UserResponse.Create(activityLog.Actor),
                Actee = UserResponse.Create(activityLog?.Actee),
                ObjectId = activityLog?.ObjectId,
                ObjectType = activityLog?.ObjectType,
                ActionType = activityLog?.ActionType,
                ActionDescription = activityLog?.ActionDescription,
                OtherDescription = activityLog?.OtherDescription,
                Base = BaseResponse.Create(activityLog)
            };
        }
    }
}

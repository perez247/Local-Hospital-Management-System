using Application.Paginations;
using Application.Query.ActivityLogEntities.GetActivityLog;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IActivityLogRepository
    {
        Task<PaginationDto<ActivityLog>> GetActivityLogs(GetActivityLogQueryFilter filter, PaginationCommand command);
    }
}

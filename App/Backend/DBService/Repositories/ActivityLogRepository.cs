using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.ActivityLogEntities.GetActivityLog;
using Application.Query.AdmissionEntities.GetPrescriptions;
using DBService.QueryHelpers;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Repositories
{
    public class ActivityLogRepository: IActivityLogRepository
    {
        private readonly AppDBContext _context;
        public ActivityLogRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<PaginationDto<ActivityLog>> GetActivityLogs(GetActivityLogQueryFilter filter, PaginationCommand command)
        {
            var query = _context.ActivityLogs
                                .Include(x => x.Actor)
                                    .ThenInclude(x => x.Staff)
                                .Include(x => x.Actee)
                                    .ThenInclude(x => x.Staff)
                                .OrderByDescending(x => x.DateCreated)
                                .AsQueryable();

            query = ActivityLogQueryHelper.FilterActivityLog(query, filter);

            return await query.GenerateEntity(command);
        }
    }
}

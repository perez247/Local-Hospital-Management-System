using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Utilities;
using Application.Query.TicketEntities.GetTickets;
using Application.Query.AdmissionEntities.GetPrescriptions;
using Application.Query.ActivityLogEntities.GetActivityLog;

namespace DBService.QueryHelpers
{
    public static class ActivityLogQueryHelper
    {
        public static IQueryable<ActivityLog> FilterActivityLog(IQueryable<ActivityLog> query, GetActivityLogQueryFilter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (filter.ActorId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.ActorId.ToString() == filter.ActorId);
            }

            if (filter.ActeeId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.ActeeId.ToString() == filter.ActeeId);
            }

            if (!string.IsNullOrEmpty(filter.ObjectType))
            {
                query = query.Where(x => x.ObjectType == filter.ObjectType);
            }

            if (!string.IsNullOrEmpty(filter.ObjectId))
            {
                query = query.Where(x => x.ObjectId == filter.ObjectId);
            }

            if (!string.IsNullOrEmpty(filter.ActionType))
            {
                query = query.Where(x => x.ActionType == filter.ActionType);
            }

            return query;

        }

    }
}

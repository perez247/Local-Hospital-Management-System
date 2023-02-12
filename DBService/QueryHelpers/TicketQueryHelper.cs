using Application.Query.GetInventories;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Query.GetTickets;

namespace DBService.QueryHelpers
{
    public static class TicketQueryHelper
    {
        public static IQueryable<AppTicket> FilterTicket(IQueryable<AppTicket> query, GetTicketsQueryFilter filter)
        {
            if (filter == null)
            {
                return query;
            }


            if (filter.AppointmentId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.AppointmentId.ToString() == filter.AppointmentId);
            }

            return query;
        }

    }
}

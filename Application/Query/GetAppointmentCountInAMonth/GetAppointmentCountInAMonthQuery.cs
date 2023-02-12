using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.GetAppointmentCountInAMonth
{
    public class GetAppointmentCountInAMonthQuery : TokenCredentials, IRequest<IEnumerable<AppointmentDayCountResponse>>
    {
        public DateTime? Date { get; set; }
    }

    public class GetAppointmentCountInAMonthHandler : IRequestHandler<GetAppointmentCountInAMonthQuery, IEnumerable<AppointmentDayCountResponse>>
    {
        private readonly IAppointmentRepository iAppointmentRepository;
        public GetAppointmentCountInAMonthHandler(IAppointmentRepository AppointmentRepository)
        {
            iAppointmentRepository = AppointmentRepository;
        }
        public async Task<IEnumerable<AppointmentDayCountResponse>> Handle(GetAppointmentCountInAMonthQuery request, CancellationToken cancellationToken)
        {
            var appointments = await iAppointmentRepository.AppAppointments()
                                               .Where(x => x.AppointmentDate.Month == request.Date.Value.Month && x.AppointmentDate.Year == request.Date.Value.Year)
                                               .ToListAsync();

            var appointmentCount = appointments.GroupBy(x => x.AppointmentDate.Day)
                                                           .Select(x => new AppointmentDayCountResponse
                                                           {
                                                               Total = x.Count(),
                                                               AppointmentDate = x.First().AppointmentDate,
                                                           })
                                                           .ToList();
            return appointmentCount;
        }
    }
}

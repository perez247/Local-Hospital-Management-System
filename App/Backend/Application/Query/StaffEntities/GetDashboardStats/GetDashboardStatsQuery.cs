using Application.Interfaces.IRepositories;
using Application.Responses;
using Application.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.StaffEntities.GetDashboardStats
{
    public class GetDashboardStatsQuery : TokenCredentials, IRequest<DashboardStatsResponse>
    {}

    public class GetDashboardStatsHandler : IRequestHandler<GetDashboardStatsQuery, DashboardStatsResponse>
    {
        private IStaffRepository _staffRepository { get; set; }

        public GetDashboardStatsHandler(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        public async Task<DashboardStatsResponse> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            return await _staffRepository.GetStats();
        }
    }
}

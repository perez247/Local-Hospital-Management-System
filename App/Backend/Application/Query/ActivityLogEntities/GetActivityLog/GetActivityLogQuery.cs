using Application.Annotations;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.TicketEntities.GetTickets;
using Application.Responses;
using Application.Utilities;
using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.ActivityLogEntities.GetActivityLog
{
    public class GetActivityLogQuery: TokenCredentials, IRequest<PaginationResponse<IEnumerable<ActivityLogResponse>>>
    {
        [SanitizePagination]
        public PaginationCommand? Pagination { get; set; }
        public GetActivityLogQueryFilter? Filter { get; set; }
    }

    public class GetActivityLogHandler : IRequestHandler<GetActivityLogQuery, PaginationResponse<IEnumerable<ActivityLogResponse>>>
    {
        private readonly IActivityLogRepository activityLogRepository;
        public GetActivityLogHandler(IActivityLogRepository activityLogRepository)
        {
            this.activityLogRepository = activityLogRepository;
        }

        public async Task<PaginationResponse<IEnumerable<ActivityLogResponse>>> Handle(GetActivityLogQuery request, CancellationToken cancellationToken)
        {
            var logs = await activityLogRepository.GetActivityLogs(request.Filter, request.Pagination);

            if (logs.Results.Count <= 0)
                return new PaginationResponse<IEnumerable<ActivityLogResponse>>() { PageNumber = request.Pagination.PageNumber, PageSize = request.Pagination.PageSize, totalItems = logs.totalItems };

            var responses = logs.Results.Select(x => ActivityLogResponse.Create(x));

            return request.Pagination.GenerateResponse<IEnumerable<ActivityLogResponse>, ActivityLog>(responses, logs);
        }
    }
}

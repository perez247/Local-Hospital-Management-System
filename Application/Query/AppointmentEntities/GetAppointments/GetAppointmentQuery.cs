using Application.Annotations;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Responses;
using Application.Utilities;
using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.AppointmentEntities.GetAppointments
{
    public class GetAppointmentQuery : TokenCredentials, IRequest<PaginationResponse<IEnumerable<AppointmentResponse>>>
    {
        [SanitizePagination]
        public PaginationCommand? Pagination { get; set; }
        public GetAppoinmentFilter? Filter { get; set; }
    }

    public class GetAppointmentHandler : IRequestHandler<GetAppointmentQuery, PaginationResponse<IEnumerable<AppointmentResponse>>>
    {
        private readonly IAppointmentRepository iAppointmentRepository;

        public GetAppointmentHandler(IAppointmentRepository AppointmentRepository)
        {
            iAppointmentRepository = AppointmentRepository;
        }
        public async Task<PaginationResponse<IEnumerable<AppointmentResponse>>> Handle(GetAppointmentQuery request, CancellationToken cancellationToken)
        {
            var usersFromDb = await iAppointmentRepository.GetAppointmentByDate(request.Filter, request.Pagination);

            if (usersFromDb.Results.Count <= 0)
                return new PaginationResponse<IEnumerable<AppointmentResponse>>() { PageNumber = request.Pagination.PageNumber, PageSize = request.Pagination.PageSize, totalItems = usersFromDb.totalItems };

            var responses = usersFromDb.Results.Select(x => AppointmentResponse.Create(x));

            return request.Pagination.GenerateResponse(responses, usersFromDb);
        }
    }
}

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

namespace Application.Query.AdmissionEntities.GetPrescriptions
{
    public class GetPrescriptionsQuery : TokenCredentials, IRequest<PaginationResponse<IEnumerable<AdmissionPrescriptionResponse>>>
    {
        [SanitizePagination]
        public PaginationCommand? Pagination { get; set; }
        public GetPrescriptionQueryFilter? Filter { get; set; }
    }

    public class GetPrescriptionHandler : IRequestHandler<GetPrescriptionsQuery, PaginationResponse<IEnumerable<AdmissionPrescriptionResponse>>>
    {
        private readonly ITicketRepository iTicketRepository;
        public GetPrescriptionHandler(ITicketRepository ITicketRepository)
        {
            iTicketRepository = ITicketRepository;
        }
        public async Task<PaginationResponse<IEnumerable<AdmissionPrescriptionResponse>>> Handle(GetPrescriptionsQuery request, CancellationToken cancellationToken)
        {
            var result = await iTicketRepository.GetAdmissionPrescription(request.Filter, request.Pagination);

            if (result.Results.Count <= 0)
            {
                return new PaginationResponse<IEnumerable<AdmissionPrescriptionResponse>>() { PageNumber = request.Pagination.PageNumber, PageSize = request.Pagination.PageSize, totalItems = result.totalItems };
            }

            var responses = result.Results.Select(x => AdmissionPrescriptionResponse.Create(x));

            return request.Pagination.GenerateResponse<IEnumerable<AdmissionPrescriptionResponse>, AdmissionPrescription>(responses, result);
        }
    }
}

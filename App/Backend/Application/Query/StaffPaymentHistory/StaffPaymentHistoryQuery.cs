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

namespace Application.Query.StaffPaymentHistory
{
    public class StaffPaymentHistoryQuery : TokenCredentials, IRequest<PaginationResponse<IEnumerable<SalaryPaymentHistoryResponse>>>
    {
        [SanitizePagination]
        public PaginationCommand? Pagination { get; set; }
        public StaffPaymentHistoryFilter? Filter { get; set; }
    }

    public class StaffPaymentHistoryHandler : IRequestHandler<StaffPaymentHistoryQuery, PaginationResponse<IEnumerable<SalaryPaymentHistoryResponse>>>
    {
        private readonly IStaffRepository iStaffRepository;

        public StaffPaymentHistoryHandler(IStaffRepository IStaffRepository)
        {
            iStaffRepository = IStaffRepository;
        }
        public async Task<PaginationResponse<IEnumerable<SalaryPaymentHistoryResponse>>> Handle(StaffPaymentHistoryQuery request, CancellationToken cancellationToken)
        {
            var result = await iStaffRepository.GetStaffListWithPayment(request.Filter, request.Pagination);

            if (result.Results.Count <= 0)
                return new PaginationResponse<IEnumerable<SalaryPaymentHistoryResponse>>() { PageNumber = request.Pagination.PageNumber, PageSize = request.Pagination.PageSize, totalItems = result.totalItems };

            var responses = result.Results.Select(x => SalaryPaymentHistoryResponse.Create(x));

            return request.Pagination.GenerateResponse<IEnumerable<SalaryPaymentHistoryResponse>, SalaryPaymentHistory>(responses, result);
        }
    }
}

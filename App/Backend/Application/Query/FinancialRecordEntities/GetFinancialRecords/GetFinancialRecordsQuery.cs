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

namespace Application.Query.FinancialRecordEntities.GetFinancialRecords
{
    public class GetFinancialRecordsQuery : TokenCredentials, IRequest<PaginationResponse<IEnumerable<FinancialRecordResponse>>>
    {
        [SanitizePagination]
        public PaginationCommand? Pagination { get; set; }
        public GetFinancialRecordsFilter? Filter { get; set; }
    }

    public class GetFinancialRecordsHandler : IRequestHandler<GetFinancialRecordsQuery, PaginationResponse<IEnumerable<FinancialRecordResponse>>>
    {
        private IFinancialRespository _financialRespository { get; set; }
        private IDBRepository _dBRepository { get; set; }

        public GetFinancialRecordsHandler(IDBRepository dBRepository, IFinancialRespository financialRespository = null)
        {
            _dBRepository = dBRepository;
            _financialRespository = financialRespository;
        }

        public async Task<PaginationResponse<IEnumerable<FinancialRecordResponse>>> Handle(GetFinancialRecordsQuery request, CancellationToken cancellationToken)
        {
            var result = await _financialRespository.GetFinancialRecords(request.Filter, request.Pagination);

            if (result.Results.Count <= 0)
                return new PaginationResponse<IEnumerable<FinancialRecordResponse>>() { PageNumber = request.Pagination.PageNumber, PageSize = request.Pagination.PageSize, totalItems = result.totalItems };

            var responses = result.Results.Select(x => FinancialRecordResponse.Create(x));

            return request.Pagination.GenerateResponse<IEnumerable<FinancialRecordResponse>, FinancialRecord>(responses, result);

        }
    }
}

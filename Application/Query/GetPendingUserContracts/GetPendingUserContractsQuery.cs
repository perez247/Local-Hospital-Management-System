using Application.Annotations;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.GetInventoryItems;
using Application.Utilities;
using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.GetPendingUserContracts
{
    public class GetPendingUserContractsQuery: TokenCredentials, IRequest<PaginationResponse<IEnumerable<GetPendingUserContractsResponse>>>
    {
        [SanitizePagination]
        public PaginationCommand? Pagination { get; set; }
        public GetPendingUserContractsFilter? Filter { get; set; }
    }

    public class GetPendingUserContractsHandler : IRequestHandler<GetPendingUserContractsQuery, PaginationResponse<IEnumerable<GetPendingUserContractsResponse>>>
    {
        private readonly IFinancialRespository _financialRespository;

        public GetPendingUserContractsHandler(IFinancialRespository financialRespository)
        {
            _financialRespository = financialRespository;
        }

        public async Task<PaginationResponse<IEnumerable<GetPendingUserContractsResponse>>> Handle(GetPendingUserContractsQuery request, CancellationToken cancellationToken)
        {
            var result = await _financialRespository.GetContracts(request.Filter, request.Pagination);

            if (result.Results.Count <= 0)
                return new PaginationResponse<IEnumerable<GetPendingUserContractsResponse>>() { PageNumber = request.Pagination.PageNumber, PageSize = request.Pagination.PageSize, totalItems = result.totalItems };
            
            var responses = result.Results.Select(x => GetPendingUserContractsResponse.Create(x));

            return request.Pagination.GenerateResponse<IEnumerable<GetPendingUserContractsResponse>, AppCost>(responses, result);
        }
    }
}

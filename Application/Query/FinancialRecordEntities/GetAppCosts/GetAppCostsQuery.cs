using Application.Annotations;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.FinancialRecordEntities.GetPendingUserContracts;
using Application.Responses;
using Application.Utilities;
using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.FinancialRecordEntities.GetAppCosts
{
    public class GetAppCostsQuery : TokenCredentials, IRequest<PaginationResponse<IEnumerable<AppCostResponse>>>
    {
        [SanitizePagination]
        public PaginationCommand? Pagination { get; set; }
        public GetAppCostFilter? Filter { get; set; }
    }

    public class GetAppCostsHandler : IRequestHandler<GetAppCostsQuery, PaginationResponse<IEnumerable<AppCostResponse>>>
    {
        private IFinancialRespository _financialRespository { get; set; }

        public GetAppCostsHandler(IFinancialRespository financialRespository)
        {
            _financialRespository = financialRespository;
        }

        public async Task<PaginationResponse<IEnumerable<AppCostResponse>>> Handle(GetAppCostsQuery request, CancellationToken cancellationToken)
        {
            var result = await _financialRespository.GetAppCostForDebts(request.Filter, request.Pagination);

            if (result.Results.Count <= 0)
                return new PaginationResponse<IEnumerable<AppCostResponse>>() { PageNumber = request.Pagination.PageNumber, PageSize = request.Pagination.PageSize, totalItems = result.totalItems };

            var responses = result.Results.Select(x => AppCostResponse.Create(x));

            return request.Pagination.GenerateResponse<IEnumerable<AppCostResponse>, AppCost>(responses, result);
        }
    }
}

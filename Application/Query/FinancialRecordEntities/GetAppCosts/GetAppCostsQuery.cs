using Application.Annotations;
using Application.DTOs;
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
    public class GetAppCostsQuery : TokenCredentials, IRequest<PaginationResponse<FinancialDebtDTO>>
    {
        [SanitizePagination]
        public PaginationCommand? Pagination { get; set; }
        public GetAppCostFilter? Filter { get; set; }
    }

    public class GetAppCostsHandler : IRequestHandler<GetAppCostsQuery, PaginationResponse<FinancialDebtDTO>>
    {
        private IFinancialRespository _financialRespository { get; set; }

        public GetAppCostsHandler(IFinancialRespository financialRespository)
        {
            _financialRespository = financialRespository;
        }

        public async Task<PaginationResponse<FinancialDebtDTO>> Handle(GetAppCostsQuery request, CancellationToken cancellationToken)
        {
            var result = await _financialRespository.GetAppCostForDebts(request.Filter, request.Pagination);

            if (result.Results.Count <= 0)
                return new PaginationResponse<FinancialDebtDTO>() { PageNumber = request.Pagination.PageNumber, PageSize = request.Pagination.PageSize, totalItems = result.totalItems };

            var data = result.Results.First();

            data.Result = data.AppCosts.Select(x => AppCostResponse.Create(x));

            data.AppCosts = null;

            return new PaginationResponse<FinancialDebtDTO>
            {
                PageNumber = request.Pagination.PageNumber,
                PageSize = request.Pagination.PageSize,
                Result = data,
                totalItems = result.totalItems
            };
        }
    }
}

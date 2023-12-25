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

namespace Application.Query.InventoryEntities.GetInventories
{
    public class GetInventoriesQuery : TokenCredentials, IRequest<PaginationResponse<IEnumerable<InventoryResponse>>>
    {
        [SanitizePagination]
        public PaginationCommand? Pagination { get; set; }
        public GetInventoriesFilter? Filter { get; set; }
    }

    public class GetInventoryHandler : IRequestHandler<GetInventoriesQuery, PaginationResponse<IEnumerable<InventoryResponse>>>
    {
        private readonly IInventoryRepository iInventoryRepository;

        public GetInventoryHandler(IInventoryRepository IInventoryRepository)
        {
            iInventoryRepository = IInventoryRepository;
        }
        public async Task<PaginationResponse<IEnumerable<InventoryResponse>>> Handle(GetInventoriesQuery request, CancellationToken cancellationToken)
        {
            var inventoriesFromDb = await iInventoryRepository.GetInventoryList(request.Filter, request.Pagination);

            if (inventoriesFromDb.Results.Count <= 0)
                return new PaginationResponse<IEnumerable<InventoryResponse>>() { PageNumber = request.Pagination.PageNumber, PageSize = request.Pagination.PageSize, totalItems = inventoriesFromDb.totalItems };

            var responses = inventoriesFromDb.Results.Select(x => InventoryResponse.Create(x));

            return request.Pagination.GenerateResponse<IEnumerable<InventoryResponse>, AppInventory>(responses, inventoriesFromDb);
        }
    }
}

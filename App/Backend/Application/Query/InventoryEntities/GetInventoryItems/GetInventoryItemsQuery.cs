using Application.Annotations;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Utilities;
using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.InventoryEntities.GetInventoryItems
{
    public class GetInventoryItemsQuery : TokenCredentials, IRequest<PaginationResponse<IEnumerable<AppInventoryItemResponse>>>
    {
        [SanitizePagination]
        public PaginationCommand? Pagination { get; set; }
        public GetInventoryItemFilter? Filter { get; set; }
    }

    public class GetInventoryItemsHandler : IRequestHandler<GetInventoryItemsQuery, PaginationResponse<IEnumerable<AppInventoryItemResponse>>>
    {
        private readonly IInventoryRepository iInventoryRepository;

        public GetInventoryItemsHandler(IInventoryRepository IInventoryRepository)
        {
            iInventoryRepository = IInventoryRepository;
        }
        public async Task<PaginationResponse<IEnumerable<AppInventoryItemResponse>>> Handle(GetInventoryItemsQuery request, CancellationToken cancellationToken)
        {
            var inventoryItemromDb = await iInventoryRepository.GetInventoryItemList(request.Filter, request.Pagination);

            if (inventoryItemromDb.Results.Count <= 0)
                return new PaginationResponse<IEnumerable<AppInventoryItemResponse>>() { PageNumber = request.Pagination.PageNumber, PageSize = request.Pagination.PageSize, totalItems = inventoryItemromDb.totalItems };

            var responses = inventoryItemromDb.Results.Select(x => AppInventoryItemResponse.Create(x));

            return request.Pagination.GenerateResponse<IEnumerable<AppInventoryItemResponse>, AppInventoryItem>(responses, inventoryItemromDb);
        }
    }
}

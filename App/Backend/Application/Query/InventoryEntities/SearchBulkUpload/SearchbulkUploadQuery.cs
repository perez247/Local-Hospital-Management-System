using Application.Annotations;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.InventoryEntities.GetInventoryItems;
using Application.Utilities;
using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.InventoryEntities.SearchBulkUpload
{
    public class SearchbulkUploadQuery: TokenCredentials, IRequest<PaginationResponse<IEnumerable<AppInventoryItemResponse>>>
    {
        [VerifyGuidAnnotation]
        public string? CompanyId { get; set; }

        [SanitizePagination]
        public PaginationCommand? Pagination { get; set; }
        public List<string>? InventoryItemNames { get; set; }
    }

    public class SearchbulkUploadHandler : IRequestHandler<SearchbulkUploadQuery, PaginationResponse<IEnumerable<AppInventoryItemResponse>>>
    {
        private readonly IInventoryRepository iInventoryRepository;
        public SearchbulkUploadHandler(IInventoryRepository IInventoryRepository)
        {
            iInventoryRepository = IInventoryRepository;
        }
        public async Task<PaginationResponse<IEnumerable<AppInventoryItemResponse>>> Handle(SearchbulkUploadQuery request, CancellationToken cancellationToken)
        {
            if (request.InventoryItemNames == null || request.InventoryItemNames.Count == 0)
            {
                request.InventoryItemNames = new List<string>();
            }

            var inventoryItemromDb = await iInventoryRepository.SearchTickeyInventoriesByName(request.CompanyId, request.InventoryItemNames);

            if (inventoryItemromDb.Results.Count <= 0)
                return new PaginationResponse<IEnumerable<AppInventoryItemResponse>>() { PageNumber = 1, PageSize = 100, totalItems = inventoryItemromDb.totalItems };

            var responses = inventoryItemromDb.Results.Select(x => AppInventoryItemResponse.Create(x));

            return request.Pagination.GenerateResponse<IEnumerable<AppInventoryItemResponse>, AppInventoryItem>(responses, inventoryItemromDb);

        }
    }
}

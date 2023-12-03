using Application.Annotations;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.InventoryEntities.GetInventoryItems;
using Application.Responses;
using Application.Utilities;
using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.InventoryEntities.GetTicketInventories
{
    public class GetTicketInventoriesQuery: TokenCredentials, IRequest<PaginationResponse<IEnumerable<TicketInventoryResponse>>>
    {
        [SanitizePagination]
        public PaginationCommand? Pagination { get; set; }
        public GetTicketInventoriesFilter? Filter { get; set; }
    }

    public class GetTicketInventoriesHandler : IRequestHandler<GetTicketInventoriesQuery, PaginationResponse<IEnumerable<TicketInventoryResponse>>>
    {
        private readonly IInventoryRepository iInventoryRepository;

        public GetTicketInventoriesHandler(IInventoryRepository IInventoryRepository)
        {
            iInventoryRepository = IInventoryRepository;
        }

        public async Task<PaginationResponse<IEnumerable<TicketInventoryResponse>>> Handle(GetTicketInventoriesQuery request, CancellationToken cancellationToken)
        {
            request.Filter.roles = request.getCurrentUserRequest().CurrentUser.UserRoles.Select(x => x.Role.Name).ToList();
            var inventoriesFromDB = await iInventoryRepository.GetTickeyInventories(request.Filter, request.Pagination);

            if (inventoriesFromDB.Results.Count <= 0)
                return new PaginationResponse<IEnumerable<TicketInventoryResponse>>() { PageNumber = request.Pagination.PageNumber, PageSize = request.Pagination.PageSize, totalItems = inventoriesFromDB.totalItems };

            var responses = inventoriesFromDB.Results.Select(x => TicketInventoryResponse.Create(x));

            return request.Pagination.GenerateResponse<IEnumerable<TicketInventoryResponse>, TicketInventory>(responses, inventoriesFromDB);

        }
    }
}

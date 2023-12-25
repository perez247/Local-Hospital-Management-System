using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Query.InventoryEntities.GetInventoryItems;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.InventoryEntities.GetInventoryItemAmount
{
    public class GetInventoryItemAmountQuery : TokenCredentials, IRequest<IEnumerable<AppInventoryItemResponse>>
    {
        [VerifyGuidAnnotation]
        public string? CompanyId { get; set; }
        public ICollection<GetInventoryItemAmountRequest>? AppInventories { get; set; }
    }

    public class GetInventoryItemAmountHandler : IRequestHandler<GetInventoryItemAmountQuery, IEnumerable<AppInventoryItemResponse>>
    {
        private readonly IInventoryRepository iInventoryRepository;

        public GetInventoryItemAmountHandler(IInventoryRepository IInventoryRepository)
        {
            iInventoryRepository = IInventoryRepository;
        }
        public async Task<IEnumerable<AppInventoryItemResponse>> Handle(GetInventoryItemAmountQuery request, CancellationToken cancellationToken)
        {
            request.AppInventories = request.AppInventories.DistinctBy(x => x.AppInventoryId).ToList();
            var ids = request.AppInventories.Select(x => x.AppInventoryId);
            var inventories = await iInventoryRepository.AppInventoryItems()
                                    .Include(x => x.AppInventory)
                                    .Where(x => ids.Contains(x.AppInventoryId.ToString()) && x.CompanyId.ToString() == request.CompanyId)
                                    .ToListAsync();

            if (inventories == null || inventories.Count == 0)
            {
                throw new CustomMessageException("No inventory found");
            }

            return inventories.Select(x => AppInventoryItemResponse.Create(x));
        }
    }
}

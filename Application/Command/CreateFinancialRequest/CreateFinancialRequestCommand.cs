using Application.Annotations;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.CreateFinancialRequest
{
    public class CreateFinancialRequestCommand : TokenCredentials, IRequest<Unit>
    {
        public decimal? Amount { get; set; }
        public string? AppCostType { get; set; }
        public string? Description { get; set; }
    }

    public class CreateFinancialRequestHandler : IRequestHandler<CreateFinancialRequestCommand, Unit>
    {
        private readonly ITicketRepository iTicketRepository;
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public CreateFinancialRequestHandler(ITicketRepository ITicketRepository, IDBRepository IDBRepository, IInventoryRepository IInventoryRepository)
        {
            iTicketRepository = ITicketRepository;
            iDBRepository = IDBRepository;
            iInventoryRepository = IInventoryRepository;
        }
        public async Task<Unit> Handle(CreateFinancialRequestCommand request, CancellationToken cancellationToken)
        {
            var newFinancialRequest = new FinancialRequest
            {
                Id = Guid.NewGuid(),
                Amount = request.Amount,
                AppCostType = request.AppCostType.ParseEnum<AppCostType>(),
                Description = request.Description,
            };

            await iDBRepository.AddAsync<FinancialRequest>(newFinancialRequest);
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}

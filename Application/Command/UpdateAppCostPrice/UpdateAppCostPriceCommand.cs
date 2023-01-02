using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdateAppCostPrice
{
    public class UpdateAppCostPriceCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? AppCostId { get; set; }
        public decimal? ApprovedPrice { get; set; }
    }

    public class UpdateAppCostPriceHandler : IRequestHandler<UpdateAppCostPriceCommand, Unit>
    {
        private readonly IStaffRepository iStaffRepository;
        private readonly IDBRepository iDBRepository;

        public UpdateAppCostPriceHandler(IStaffRepository IStaffRepository, IDBRepository IDBRepository)
        {
            iStaffRepository = IStaffRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<Unit> Handle(UpdateAppCostPriceCommand request, CancellationToken cancellationToken)
        {
            var appCost = await iStaffRepository.AppCosts()
                                                .Include(x => x.AppTicket)
                                                .FirstOrDefaultAsync(x => x.Id.ToString() == request.AppCostId);
            
            if (appCost == null)
            {
                throw new CustomMessageException("Cost not found");
            }

            if (appCost.AppTicket.AppTicketStatus == AppTicketStatus.concluded || appCost.AppTicket.AppTicketStatus == AppTicketStatus.canceled)
            {
                throw new CustomMessageException("This ticket has been concluded or canceled");
            }

            var payments = appCost.Payments;

            var sum = payments.Sum(x => x.Amount);

            if (sum > request.ApprovedPrice)
            {
                throw new CustomMessageException("Amount cannot be smaller than the paid so far");
            }

            appCost.ApprovedPrice = request.ApprovedPrice;

            var appTicket = appCost.AppTicket;

            iDBRepository.Update<AppCost>(appCost);
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}

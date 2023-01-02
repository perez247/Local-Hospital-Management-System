using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdatePaymentForMonth
{
    public class UpdatePaymentForMonthCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidCollectionAnnotation]
        public ICollection<string>? StaffIds { get; set; }
        public DateTime? Date { get; set; }
        public bool? Paid { get; set; }
    }

    public class UpdatePaymentForMonthHandler : IRequestHandler<UpdatePaymentForMonthCommand, Unit>
    {
        private readonly IStaffRepository iStaffRepository;
        private readonly IDBRepository iDBRepository;

        public UpdatePaymentForMonthHandler(IStaffRepository IStaffRepository, IDBRepository IDBRepository)
        {
            iStaffRepository = IStaffRepository;
            iDBRepository = IDBRepository;
        }

        public async Task<Unit> Handle(UpdatePaymentForMonthCommand request, CancellationToken cancellationToken)
        {
            request.StaffIds = request.StaffIds != null && request.StaffIds.Count > 0 ? request.StaffIds.Distinct().ToList() : new List<string>();
            
            var staffToPay = await iStaffRepository.Staff()
                                                   .Include(x => x.SalaryPaymentHistory.Where(y => y.DatePaidFor.Month == request.Date.Value.Month && y.DatePaidFor.Year == request.Date.Value.Year))
                                                   .Where(y => y.SalaryPaymentHistory != null && y.SalaryPaymentHistory.Count > 0 && request.StaffIds.Contains(y.Id.ToString()))
                                                   .ToListAsync();

            if (staffToPay.Count == 0)
            {
                throw new CustomMessageException("No staff found to pay");
            }

            var salaries = staffToPay.Select(x =>
            {
                var salaryToUpdate = x.SalaryPaymentHistory.First();
                salaryToUpdate.Amount = x.Salary.Value;
                return salaryToUpdate;

            });

            foreach (var salary in salaries)
            {
                salary.Paid = request.Paid.Value;
                salary.DatePaidFor = request.Date.Value;
                salary.Savings = salary.Amount * 0.10m;
                salary.Amount = salary.Savings - salary.Amount;
            }

            iDBRepository.UpdateRange<SalaryPaymentHistory>(salaries.ToList());
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}

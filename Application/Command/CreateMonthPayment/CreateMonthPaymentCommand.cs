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

namespace Application.Command.CreateMonthPayment
{
    public class CreateMonthPaymentCommand : TokenCredentials, IRequest<Unit>
    {
        public DateTime? Date { get; set; }
    }

    public class CreateMonthPaymentHandler : IRequestHandler<CreateMonthPaymentCommand, Unit>
    {
        private readonly IStaffRepository iStaffRepository;
        private readonly IDBRepository iDBRepository;

        public CreateMonthPaymentHandler(IStaffRepository IStaffRepository, IDBRepository IDBRepository)
        {
            iStaffRepository = IStaffRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<Unit> Handle(CreateMonthPaymentCommand request, CancellationToken cancellationToken)
        {
            var staffwithEmptyMonth = await iStaffRepository.Staff()
                                                            .Include(x => x.SalaryPaymentHistory.Where(y => y.DatePaidFor.Month == request.Date.Value.Month && y.DatePaidFor.Year == request.Date.Value.Year))
                                                            .Where(s => s.SalaryPaymentHistory != null && s.SalaryPaymentHistory.Count <= 0)
                                                            .ToListAsync();

            if (staffwithEmptyMonth.Count == 0)
            {
                throw new CustomMessageException("No user to add month");
            }

            var newSalaryMonth = staffwithEmptyMonth.Select(x =>
            {
                return new SalaryPaymentHistory
                {
                    StaffId = x.Id,
                    DatePaidFor = request.Date.Value,
                };
            });

            await iDBRepository.AddRangeAsync<SalaryPaymentHistory>(newSalaryMonth);
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}

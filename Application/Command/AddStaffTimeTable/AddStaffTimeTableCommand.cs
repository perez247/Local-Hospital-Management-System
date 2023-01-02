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

namespace Application.Command.AddStaffTimeTable
{
    public class AddStaffTimeTableCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidCollectionAnnotation]
        public ICollection<string>? StaffIds { get; set; }
        public DateTime? ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }
    }

    public class AddStaffTimeTableHandler : IRequestHandler<AddStaffTimeTableCommand, Unit>
    {
        private readonly IStaffRepository iStaffRepository;
        private readonly IDBRepository iDBRepository;

        public AddStaffTimeTableHandler(IStaffRepository IStaffRepository, IDBRepository IDBRepository)
        {
            iStaffRepository = IStaffRepository;
            iDBRepository = IDBRepository;
        }

        public async Task<Unit> Handle(AddStaffTimeTableCommand request, CancellationToken cancellationToken)
        {

            request.StaffIds = request.StaffIds != null && request.StaffIds.Count > 0 ? request.StaffIds.Distinct().ToList() : new List<string>();

            var staffInDB = await iStaffRepository.Staff()
                                                  .Include(x => x.TimeTable.Where(y => y.ClockIn == request.ClockIn && y.ClockOut == request.ClockOut))
                                                  .Where(x => (x.TimeTable == null || x.TimeTable.Count() == 0) && request.StaffIds.Contains(x.Id.ToString()))
                                                  .ToListAsync();

            if (staffInDB.Count == 0)
            {
                throw new CustomMessageException("No staff to assign to shift");
            }

            var timeTable = staffInDB.Select(x =>
            {
                return new StaffTimeTable
                {
                    StaffId = x.Id,
                    ClockIn = request.ClockIn.Value,
                    ClockOut = request.ClockOut.Value,
                };
            });

            await iDBRepository.AddRangeAsync<StaffTimeTable>(timeTable.ToList());
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}

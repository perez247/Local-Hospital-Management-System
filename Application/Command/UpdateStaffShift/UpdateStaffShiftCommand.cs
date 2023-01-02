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
using System.Xml;

namespace Application.Command.UpdateStaffShift
{
    public class UpdateStaffShiftCommand : TokenCredentials, IRequest<Unit>
    {
        public ICollection<UpdateStaffShiftRequest>? Shifts { get; set; }
        public DateTime? ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }
    }

    public class UpdateStaffShiftHandler : IRequestHandler<UpdateStaffShiftCommand, Unit>
    {
        private readonly IStaffRepository iStaffRepository;
        private readonly IDBRepository iDBRepository;

        public UpdateStaffShiftHandler(IStaffRepository IStaffRepository, IDBRepository IDBRepository)
        {
            iStaffRepository = IStaffRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<Unit> Handle(UpdateStaffShiftCommand request, CancellationToken cancellationToken)
        {
            request.Shifts = request.Shifts.DistinctBy(x => x.StaffId).ToList();

            var staffIds = request.Shifts.Select(x => x.StaffId);

            var staffInDB = await iStaffRepository.Staff()
                                                  .Include(x => x.TimeTable.Where(y => y.ClockIn == request.ClockIn && y.ClockOut == request.ClockOut))
                                                  .Where(x => x.TimeTable != null && x.TimeTable.Count() > 0 && staffIds.Contains(x.Id.ToString()))
                                                  .ToListAsync();


            if (staffInDB.Count == 0)
            {
                throw new CustomMessageException("No staff to update shift");
            }

            var timeTables = staffInDB.Select(x =>
            {
                var y = x.TimeTable.First();
                var shift = request.Shifts.First(y => y.StaffId == x.Id.ToString());
                y.StaffClockIn = shift.ClockIn;
                y.StaffClockOut = shift.ClockOut;
                return y;
            });


            iDBRepository.UpdateRange<StaffTimeTable>(timeTables.ToList());
            await iDBRepository.Complete();


            return Unit.Value;
        }
    }
}

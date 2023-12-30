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

namespace Application.Command.StaffEntities.UpdateStaffDetails
{
    public class UpdateStaffDetailCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? StaffId { get; set; }
        public string? Level { get; set; }
        //public string? ContractStaff { get; set; }
        public decimal? Salary { get; set; }
        public string? Position { get; set; }
        public bool? Active { get; set; }
        public string? AccountNumber { get; set; }
        public string? BankName { get; set; }
        public string? BankId { get; set; }
    }

    public class UpdateStaffDetailHandler : IRequestHandler<UpdateStaffDetailCommand, Unit>
    {
        private readonly IUserRepository iUserRepository;
        private readonly IDBRepository iDBRepository;

        public UpdateStaffDetailHandler(IUserRepository IUserRepository, IDBRepository IDBRepository)
        {
            iUserRepository = IUserRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<Unit> Handle(UpdateStaffDetailCommand request, CancellationToken cancellationToken)
        {
            var user = await iUserRepository.Users()
                                            .Include(x => x.UserRoles)
                                                .ThenInclude(x => x.Role)
                                            .Include(x => x.Staff)
                                            .FirstOrDefaultAsync(x => x.Staff != null && x.Staff.Id.ToString() == request.StaffId);

            if (user == null)
            {
                throw new CustomMessageException("Staff not found", System.Net.HttpStatusCode.NotFound);
            }

            var staff = user.Staff;

            staff.Level = request.Level.Trim();
            staff.Salary = request.Salary;
            staff.Position = request.Position;
            staff.Active = request.Active.HasValue ? request.Active.Value : false;
            staff.AccountNumber = string.IsNullOrEmpty(request.AccountNumber) ? null : request.AccountNumber.Trim();
            staff.BankName = string.IsNullOrEmpty(request.BankName) ? null : request.BankName.Trim();
            staff.BankId = string.IsNullOrEmpty(request.BankId) ? null : request.BankId.Trim();

            if (!staff.Active)
            {
                var roles = user.UserRoles.Select(x => x.Role);
                var hasAdmin = roles.FirstOrDefault(x => x.Name == StaffRoleEnum.admin.ToString());

                if (hasAdmin != null)
                {
                    throw new CustomMessageException("User is admin, kindly remove admin privilege before making user inactive");
                }
            }


            iDBRepository.Update(staff);
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}

using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.StaffEntities.UpdateStaffRoles
{
    public class UpdateStaffRolesCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? UserId { get; set; }

        public ICollection<string>? StaffRoleEnum { get; set; }
    }

    public class UpdateStaffRolesHandler : IRequestHandler<UpdateStaffRolesCommand, Unit>
    {
        private readonly IUserRepository iUserRepository;
        private readonly IAuthRepository iAuthRepository;
        private readonly IDBRepository iDBRepository;

        public UpdateStaffRolesHandler(IUserRepository IUserRepository, IAuthRepository IAuthRepository, IDBRepository IDBRepository)
        {
            iUserRepository = IUserRepository;
            iAuthRepository = IAuthRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<Unit> Handle(UpdateStaffRolesCommand request, CancellationToken cancellationToken)
        {
            var user = await iUserRepository.Users()
                            .Include(x => x.UserRoles)
                                .ThenInclude(x => x.Role)
                            .FirstOrDefaultAsync(x => x.Id.ToString() == request.UserId);

            if (user == null)
            {
                throw new CustomMessageException("Staff not found");
            }

            var isAdmin = user.UserRoles.FirstOrDefault(x => x.Role.Name == StaffRoleEnum.admin.ToString());
            var noAdminFoundInRequest = !request.StaffRoleEnum.Contains(StaffRoleEnum.admin.ToString());

            if (isAdmin != null && noAdminFoundInRequest)
            {
                var adminUsers = await iUserRepository.UserRoles()
                    .Include(x => x.Role)
                    .Where(x => x.Role.Name == StaffRoleEnum.admin.ToString())
                    .Take(2)
                    .ToListAsync();

                if (adminUsers.Count <= 0)
                {
                    throw new CustomMessageException("No admin found, this should not be happening. Kindly contact admin");
                }

                if (adminUsers.Count < 2)
                {
                    var onlyAdmin = adminUsers.FirstOrDefault();
                    if (onlyAdmin.UserId == user.Id)
                    {
                        throw new CustomMessageException("At least one admin is required to be present");
                    }
                }
            }

            await iAuthRepository.RemoveFromAllRoles(user);
            await iAuthRepository.AddToRoles(user, request.StaffRoleEnum);

            return Unit.Value;
        }
    }
}

using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UserEntities.ChangePassword
{
    public class ChangePasswordCommand: TokenCredentials, IRequest<Unit>
    {
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }

    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly IUserRepository iUserRepository;
        private readonly IAuthRepository iAuthRepository;

        public ChangePasswordHandler(IUserRepository IUserRepository, IAuthRepository IAuthRepository)
        {
            iUserRepository = IUserRepository;
            iAuthRepository = IAuthRepository;
        }
        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var changed = await iAuthRepository.changePassword(request.getCurrentUserRequest().CurrentUser, request.OldPassword, request.NewPassword);

            if (!changed)
            {
                throw new CustomMessageException("Failed to change password, kindly try again later");
            }

            return Unit.Value;
        }
    }
}

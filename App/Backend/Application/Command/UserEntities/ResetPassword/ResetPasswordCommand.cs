using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UserEntities.ResetPassword
{
    public class ResetPasswordCommand: TokenCredentials, IRequest<ResetPasswordResponse>
    {
        [VerifyGuidAnnotation]
        public string? UserId { get; set; }
    }

    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordResponse>
    {
        private readonly IUserRepository iUserRepository;
        private readonly IAuthRepository iAuthRepository;

        public ResetPasswordHandler(IUserRepository IUserRepository, IAuthRepository IAuthRepository)
        {
            iUserRepository = IUserRepository;
            iAuthRepository = IAuthRepository;
        }
        public async Task<ResetPasswordResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await iUserRepository.Users().FirstOrDefaultAsync(x => x.Id.ToString() == request.UserId);

            if (user == null)
            {
                throw new CustomMessageException("User not found");
            }

            string password = UtilityHelper.GenerateRandomPassword();

            var result = await iAuthRepository.resetpassword(user, password);
            
            if (!result) 
            {
                throw new CustomMessageException("Failed to reset user's password");
            }

            return new ResetPasswordResponse { NewPassword = password };
        }
    }
}

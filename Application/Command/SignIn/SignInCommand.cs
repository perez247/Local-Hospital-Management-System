using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;

namespace Application.Command.SignIn
{
    public class SignInCommand : TokenCredentials, IRequest<SignInResponse>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class SignInHandler : IRequestHandler<SignInCommand, SignInResponse>
    {
        private readonly IUserRepository iUserRepository;
        private readonly IAuthRepository iAuthRepository;

        public SignInHandler(IUserRepository IUserRepository, IAuthRepository IAuthRepository)
        {
            iUserRepository = IUserRepository;
            iAuthRepository = IAuthRepository;
        }

        public async Task<SignInResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var userFromDB = await iUserRepository.GetUserByIdOrEmailAsync("", request.Email);

            if (userFromDB == null)
                throw new CustomMessageException("Hmmmm that didn't work, Try again...");

            if (userFromDB.LockoutEnd.HasValue && userFromDB.LockoutEnd.Value.UtcDateTime.ToUniversalTime() > DateTime.Now.ToUniversalTime())
                throw new CustomMessageException($"This account has been locked for now");

            // Check the password and increase failed attempts if failed
            if (!await iAuthRepository.CheckPasswordAndLockOn5FailedAttempts(userFromDB, request.Password))
                throw new CustomMessageException("Hmmmm that didn't work, Try again...");

            // Generate jwt token
            var token = TokenHelper.generateUserToken(userFromDB, request.RememberMe);

            return new SignInResponse
            {
                JWT = token
            };
        }
    }
}

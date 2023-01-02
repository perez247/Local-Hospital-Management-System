using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Models;

namespace Application.Command.CreateStaff
{
    public class CreateStaffCommand : TokenCredentials, IRequest<CreateStaffResponse>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
    }

    public class CreateStaffHandler : IRequestHandler<CreateStaffCommand, CreateStaffResponse>
    {
        private readonly IStaffRepository iStaffRepository;
        private readonly IAuthRepository iAuthRepository;

        public CreateStaffHandler(IStaffRepository IStaffRepository, IAuthRepository IAuthRepository)
        {
            iStaffRepository = IStaffRepository;
            iAuthRepository = IAuthRepository;
        }
        public async Task<CreateStaffResponse> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
        {
            var emailAvaliable = await iAuthRepository.IsEmailAvailable(request.Email);

            if (!emailAvaliable)
            {
                throw new CustomMessageException($"{request.Email} has been taken");
            }

            AppUser newUser = new AppUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                OtherName = request.OtherName,
                Address = request.Address,
                Email = request.Email,
                UserName = request.Email,
                Staff = new Staff
                {
                    Id = Guid.NewGuid(),
                },
            };

            string password = UtilityHelper.GenerateRandomPassword();

            newUser = await iStaffRepository.CreateStaff(newUser, password);
            return new CreateStaffResponse { StaffId = newUser?.Staff.Id?.ToString() ?? string.Empty, Password = password };
        }
    }
}

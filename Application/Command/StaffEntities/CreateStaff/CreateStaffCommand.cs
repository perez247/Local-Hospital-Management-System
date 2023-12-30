using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Application.Command.StaffEntities.CreateStaff
{
    public class CreateStaffCommand : TokenCredentials, IRequest<CreateStaffResponse>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }

        [VerifyGuidAnnotation]
        public string? PatientId { get; set; }
    }

    public class CreateStaffHandler : IRequestHandler<CreateStaffCommand, CreateStaffResponse>
    {
        private readonly IStaffRepository iStaffRepository;
        private readonly IPatientRepository iPatientRepository;
        private readonly IAuthRepository iAuthRepository;
        private readonly IDBRepository iDBRepository;

        public CreateStaffHandler(IDBRepository IDBRepository, IPatientRepository IPatientRepository, IStaffRepository IStaffRepository, IAuthRepository IAuthRepository)
        {
            iPatientRepository = IPatientRepository;
            iStaffRepository = IStaffRepository;
            iAuthRepository = IAuthRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<CreateStaffResponse> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
        {
            if (request.PatientId != Guid.Empty.ToString())
            {
                var patient = await iPatientRepository.Patients()
                                        .Include(x => x.AppUser)
                                            .ThenInclude(x => x.Staff)
                                        .FirstOrDefaultAsync(x => x.Id.ToString() == request.PatientId);

                if (patient == null)
                {
                    throw new CustomMessageException("Patient to add as staff not found");
                }

                var staff = patient.AppUser.Staff;

                if (staff != null)
                {
                    throw new CustomMessageException("Patient is already a staff");
                }

                var newStaff = new Staff
                {

                    Id = Guid.NewGuid(),
                    AppUserId = patient.AppUserId
                };

                await iDBRepository.AddAsync(newStaff);
                await iDBRepository.Complete();

                return new CreateStaffResponse { UserId = patient?.AppUserId.ToString() ?? string.Empty, Password = "" };

            }
            else
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

                return new CreateStaffResponse { 
                    UserId = newUser?.Id.ToString() ?? string.Empty, 
                    Password = password,
                    Email = request.Email,
                    FullName = $"{request.LastName} {request.FirstName}"
                };

            }

        }
    }
}

using Application.Annotations;
using Application.Command.CreateStaff;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.CreatePatient
{
    public class CreatePatientCommand : TokenCredentials, IRequest<CreatePatientResponse>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }

        [VerifyGuidAnnotation]
        public string? CompanyId { get; set; }
    }

    public class CreatePaitentHandler : IRequestHandler<CreatePatientCommand, CreatePatientResponse>
    {
        private readonly IPatientRepository iPatientRepository;
        private readonly IAuthRepository iAuthRepository;

        public CreatePaitentHandler(IPatientRepository IPatientRepository, IAuthRepository IAuthRepository)
        {
            iPatientRepository = IPatientRepository;
            iAuthRepository = IAuthRepository;
        }
        public async Task<CreatePatientResponse> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
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
                Patient = new Patient
                {
                    Id = Guid.NewGuid(),
                    CompanyId = request.CompanyId == Guid.Empty.ToString() ? null : Guid.Parse(request.CompanyId),
                },
            };

            string password = UtilityHelper.GenerateRandomPassword();

            newUser = await iPatientRepository.CreatePatient(newUser, password);

            // You can send patient the username and password

            return new CreatePatientResponse { PatientId = newUser?.Patient.Id?.ToString() ?? string.Empty };

        }
    }
}

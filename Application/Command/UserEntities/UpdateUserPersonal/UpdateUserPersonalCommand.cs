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

namespace Application.Command.UserEntities.UpdateUserPersonal
{
    public class UpdateUserPersonalCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? Phone { get; set; }
        public string? Occupation { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Profile { get; set; }
        public string? CompanyUniqueId { get; set; }
        public string? OtherInformation { get; set; }
    }

    public class UpdateUserPersonalHandler : IRequestHandler<UpdateUserPersonalCommand, Unit>
    {
        private readonly IUserRepository userRepository;
        private readonly IDBRepository iDBRepository;

        public UpdateUserPersonalHandler(IDBRepository IDBRepository, IUserRepository userRepository)
        {
            iDBRepository = IDBRepository;
            this.userRepository = userRepository;
        }

        public async Task<Unit> Handle(UpdateUserPersonalCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.Users().Include(x => x.Patient)
                                                   .FirstOrDefaultAsync(x => x.Id.ToString() == request.UserId);

            if (user == null)
            {
                throw new CustomMessageException("User not found", System.Net.HttpStatusCode.NotFound);
            }

            user.FirstName = request.FirstName.Trim();
            user.LastName = request.LastName.Trim();
            user.OtherName = string.IsNullOrEmpty(request.OtherName) ? null : request.OtherName.Trim();
            user.PhoneNumber = string.IsNullOrEmpty(request.Phone) ? null : request.Phone.Trim();
            user.Address = request.Address.Trim();
            user.Profile = request.Profile;
            user.Occupation = request.Occupation;
            user.Gender = request.Gender;
            iDBRepository.Update(user);

            if (!string.IsNullOrEmpty(request.CompanyUniqueId) || !string.IsNullOrEmpty(request.OtherInformation))
            {
                var save = false;
                if (user.Patient == null)
                {
                    throw new CustomMessageException("This user is not a patient");
                }

                if (user.Patient.CompanyUniqueId != request.CompanyUniqueId)
                {
                    user.Patient.CompanyUniqueId = request.CompanyUniqueId;
                    save = true;
                }

                if (user.Patient.OtherInformation != request.OtherInformation)
                {
                    user.Patient.OtherInformation = request.OtherInformation;
                    save = true;
                }

                if (save)
                {
                    iDBRepository.Update<Patient>(user.Patient);
                }
            }

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}

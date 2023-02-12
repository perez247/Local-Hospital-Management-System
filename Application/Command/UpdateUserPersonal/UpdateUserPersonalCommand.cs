using Application.Annotations;
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

namespace Application.Command.UpdateUserPersonal
{
    public class UpdateUserPersonalCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Profile { get; set; }
    }

    public class UpdateUserPersonalHandler : IRequestHandler<UpdateUserPersonalCommand, Unit>
    {
        private readonly IStaffRepository iStaffRepository;
        private readonly IDBRepository iDBRepository;

        public UpdateUserPersonalHandler(IStaffRepository IStaffRepository, IDBRepository IDBRepository)
        {
            iStaffRepository = IStaffRepository;
            iDBRepository = IDBRepository;
        }

        public async Task<Unit> Handle(UpdateUserPersonalCommand request, CancellationToken cancellationToken)
        {
            var user = await iDBRepository.FindAsync<AppUser>(x => x.Id.ToString() == request.UserId);

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

            iDBRepository.Update<AppUser>(user);
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}

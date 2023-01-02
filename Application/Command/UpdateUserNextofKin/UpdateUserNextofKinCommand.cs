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

namespace Application.Command.UpdateUserNextofKin
{
    public class UpdateUserNextofKinCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }

    public class UpdateUserNextOfKinHandler : IRequestHandler<UpdateUserNextofKinCommand, Unit>
    {
        private readonly IUserRepository iUserRepository;
        private readonly IDBRepository iDBRepository;

        public UpdateUserNextOfKinHandler(IUserRepository IUserRepository, IDBRepository IDBRepository)
        {
            iUserRepository = IUserRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<Unit> Handle(UpdateUserNextofKinCommand request, CancellationToken cancellationToken)
        {
            var user = await iUserRepository.Users().Include(x => x.NextOfKin)
                                            .FirstOrDefaultAsync(x => x.Id.ToString() == request.UserId);

            if (user == null)
            {
                throw new CustomMessageException("User not found", System.Net.HttpStatusCode.NotFound);
            }

            var nextofkin = user.NextOfKin;
            var newNextofKin = false;

            if (nextofkin == null)
            {
                nextofkin = new Models.NextOfKin { 
                    Id = Guid.NewGuid() ,
                    AppUserId = user.Id,
                };
                newNextofKin = true;
            }

            nextofkin.FirstName= request.FirstName.Trim();
            nextofkin.LastName= request.LastName.Trim();
            nextofkin.Email= string.IsNullOrEmpty(request.Email) ? null : request.Email.Trim();
            nextofkin.OtherName= string.IsNullOrEmpty(request.OtherName) ? null : request.OtherName.Trim();
            nextofkin.Phone1= request.Phone1.Trim();
            nextofkin.Phone2= string.IsNullOrEmpty(request.Phone2) ? null : request.Phone2.Trim();
            nextofkin.Address= request.Address;

            if (newNextofKin)
            {
                await iDBRepository.AddAsync<NextOfKin>(nextofkin);
            } else
            {
                iDBRepository.Update<NextOfKin>(nextofkin);
            }
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}

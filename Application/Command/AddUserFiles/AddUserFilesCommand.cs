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

namespace Application.Command.AddUserFiles
{
    public class AddUserFilesCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? UserId { get; set; }
        public ICollection<AddUserFilesRequest>? UserFiles { get; set; }
    }

    public class AddUserFilesHandler : IRequestHandler<AddUserFilesCommand, Unit>
    {
        private readonly IUserRepository iUserRepository;
        private readonly IDBRepository iDBRepository;

        public AddUserFilesHandler(IDBRepository IDBRepository, IUserRepository IUserRepository)
        {
            iDBRepository = IDBRepository;
            iUserRepository = IUserRepository;
        }

        public async Task<Unit> Handle(AddUserFilesCommand request, CancellationToken cancellationToken)
        {
            var user = await iUserRepository.Users()
                                            .Include(x => x.UserFiles)
                                            .FirstOrDefaultAsync(x => x.Id.ToString() == request.UserId);

            if (user == null)
            {
                throw new CustomMessageException($"User to update not found");
            }

            var filesInDb = user.UserFiles.Count() + request.UserFiles.Count();

            if (filesInDb > 10)
            {
                throw new CustomMessageException($"You can only have a maximum of 10 files per user");
            }

            foreach ( var file in request.UserFiles )
            {
                var fileToAdd = new UserFile()
                {
                    AppUserId = user.Id,
                    Name = file.Name,
                    Base64String = file.Base64String,
                };

                await iDBRepository.AddAsync<UserFile>( fileToAdd );

            }

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}

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

namespace Application.Command.UserEntities.DeleteUserFiles
{
    public class DeleteUserFIlesCommand : TokenCredentials, IRequest<Unit>
    {

        [VerifyGuidAnnotation]
        public string? UserId { get; set; }

        [VerifyGuidCollectionAnnotation]
        public ICollection<string>? UserFileIds { get; set; }
    }

    public class DeleteUserFIlesHandler : IRequestHandler<DeleteUserFIlesCommand, Unit>
    {
        private readonly IUserRepository iUserRepository;
        private readonly IDBRepository iDBRepository;

        public DeleteUserFIlesHandler(IDBRepository IDBRepository, IUserRepository IUserRepository)
        {
            iDBRepository = IDBRepository;
            iUserRepository = IUserRepository;
        }
        public async Task<Unit> Handle(DeleteUserFIlesCommand request, CancellationToken cancellationToken)
        {

            var filesToDelete = await iUserRepository.UserFiles()
                                        .Select(x => new UserFile
                                        {
                                            Id = x.Id,
                                            AppUserId = x.AppUserId,
                                            Name = x.Name,
                                        })
                                        .Where(x => request.UserFileIds.Contains(x.Id.ToString()))
                                        .ToListAsync();

            if (filesToDelete.Count() <= 0)
            {
                throw new CustomMessageException($"No files to delete");
            }

            foreach (var file in filesToDelete)
            {
                iDBRepository.Remove(file);
            }

            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}

using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Responses;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.UserEntities.GetUserFiles
{
    public class GetUserFIlesQuery : TokenCredentials, IRequest<IEnumerable<UserFileResponse>>
    {
        [VerifyGuidAnnotation]
        public string? UserId { get; set; }
        public string? UserFileId { get; set; }
    }

    public class GetUserFIlesHandler : IRequestHandler<GetUserFIlesQuery, IEnumerable<UserFileResponse>>
    {
        private readonly IUserRepository iUserRepository;

        public GetUserFIlesHandler(IUserRepository IUserRepository)
        {
            iUserRepository = IUserRepository;
        }

        public async Task<IEnumerable<UserFileResponse>> Handle(GetUserFIlesQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId != Guid.Empty.ToString())
            {
                var userFiles = await iUserRepository.UserFiles()
                                            .Select(x => new UserFile
                                            {
                                                Id = x.Id,
                                                AppUserId = x.AppUserId,
                                                Name = x.Name,
                                            })
                                            .Where(x => x.AppUserId.ToString() == request.UserId)
                                            .ToListAsync();

                return userFiles.Select(x => UserFileResponse.Create(x));
            }
            else
            {
                var file = await iUserRepository.UserFiles()
                                                .FirstOrDefaultAsync(x => x.Id.ToString() == request.UserFileId);

                if (file == null)
                {
                    throw new CustomMessageException($"File not found");
                }

                return new List<UserFileResponse>() { UserFileResponse.Create(file) };
            }
        }
    }
}

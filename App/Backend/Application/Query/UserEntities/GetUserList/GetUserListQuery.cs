using Application.Annotations;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Responses;
using Application.Utilities;
using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.UserEntities.GetUserList
{
    public class GetUserListQuery : TokenCredentials, IRequest<PaginationResponse<IEnumerable<UserResponse>>>
    {
        [SanitizePagination]
        public PaginationCommand? Pagination { get; set; }

        public GetUserListFilter? Filter { get; set; }
    }

    public class GetUserListHandler : IRequestHandler<GetUserListQuery, PaginationResponse<IEnumerable<UserResponse>>>
    {
        
        private readonly IUserRepository iUserRepository;

        public GetUserListHandler(IUserRepository IUserRepository)
        {
            iUserRepository = IUserRepository;
        }
        public async Task<PaginationResponse<IEnumerable<UserResponse>>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var usersFromDb = await iUserRepository.GetUserList(request.Filter, request.Pagination);

            if (usersFromDb.Results.Count <= 0)
                return new PaginationResponse<IEnumerable<UserResponse>>() { PageNumber = request.Pagination.PageNumber, PageSize = request.Pagination.PageSize, totalItems = usersFromDb.totalItems };

            var responses = usersFromDb.Results.Select(x => UserResponse.Create(x));

            return request.Pagination.GenerateResponse<IEnumerable<UserResponse>, AppUser>(responses, usersFromDb);
        }
    }
}

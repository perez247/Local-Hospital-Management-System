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

namespace Application.Query.GetStaffList
{
    public class GetStaffListQuery : TokenCredentials, IRequest<PaginationResponse<IEnumerable<UserResponse>>>
    {
        [SanitizePagination]
        public PaginationCommand? Pagination { get; set; }
        public GetStaffListFilter? Filter { get; set; }
    }

    public class GetStaffListHandler : IRequestHandler<GetStaffListQuery, PaginationResponse<IEnumerable<UserResponse>>>
    {
        private readonly IStaffRepository iStaffRepository;

        public GetStaffListHandler(IStaffRepository IStaffRepository)
        {
            iStaffRepository = IStaffRepository;
        }

        public async Task<PaginationResponse<IEnumerable<UserResponse>>> Handle(GetStaffListQuery request, CancellationToken cancellationToken)
        {
            var usersFromDb = await iStaffRepository.GetStaffList(request.Filter, request.Pagination);

            if (usersFromDb.Results.Count <= 0)
                return new PaginationResponse<IEnumerable<UserResponse>>() { PageNumber = request.Pagination.PageNumber, PageSize = request.Pagination.PageSize, totalItems = usersFromDb.totalItems };

            var responses = usersFromDb.Results.Select(x => UserResponse.Create(x));

            return request.Pagination.GenerateResponse<IEnumerable<UserResponse>, AppUser>(responses, usersFromDb);
        }
    }
}

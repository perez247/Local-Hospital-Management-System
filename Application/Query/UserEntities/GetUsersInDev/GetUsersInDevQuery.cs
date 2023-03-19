using Application.Interfaces.IRepositories;
using Application.Responses;
using Application.Utilities;
using MediatR;

namespace Application.Query.UserEntities.GetUsersInDev
{
    public class GetUsersInDevQuery : TokenCredentials, IRequest<IEnumerable<UserResponse>>
    {
    }

    public class GetUsersInDevHandler : IRequestHandler<GetUsersInDevQuery, IEnumerable<UserResponse>>
    {
        private readonly IUserRepository iUserRepository;

        public GetUsersInDevHandler(IUserRepository IUserRepository)
        {
            iUserRepository = IUserRepository;
        }
        public async Task<IEnumerable<UserResponse>> Handle(GetUsersInDevQuery request, CancellationToken cancellationToken)
        {
            var users = await iUserRepository.GetUsers();
            return users.Select(x => UserResponse.Create(x));
        }
    }
}

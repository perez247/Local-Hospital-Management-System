using Application.Paginations;
using Application.Query.UserEntities.GetUserList;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<AppUser?> GetUserByIdOrEmailAsync(string id, string email);
        Task<IEnumerable<AppUser>> GetUsers();
        IQueryable<AppUser> Users();
        IQueryable<AppUserRole> UserRoles();
        IQueryable<UserFile> UserFiles();
        IQueryable<Company> Companies();
        Task<PaginationDto<AppUser>> GetUserList(GetUserListFilter filter, PaginationCommand command);
    }
}

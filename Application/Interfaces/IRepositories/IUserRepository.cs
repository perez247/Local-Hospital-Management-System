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
        IQueryable<Company> Companies();
    }
}

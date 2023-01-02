using Application.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DBService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _context;
        public UserRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<AppUser?> GetUserByIdOrEmailAsync(string id, string email)
        {
            return await _context.Users
                                .Include(x => x.Staff)
                                .Include(x => x.Patient)
                                .Include(x => x.UserRoles).ThenInclude(y => y.Role)
                                        .FirstOrDefaultAsync(x => x.Id.ToString() == id || x.Email.ToLower().Equals(email.ToLower()));
        }

        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            return await _context.Users.Include(x => x.Staff).ToListAsync();
        }

        public IQueryable<AppUser> Users()
        {
            return _context.Users.AsQueryable();
        }

        public IQueryable<Company> Companies()
        {
            return _context.Companies.AsQueryable();
        }

    }
}

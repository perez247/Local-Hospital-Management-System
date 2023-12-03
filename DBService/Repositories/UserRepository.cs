using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.UserEntities.GetUserList;
using DBService.QueryHelpers;
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
                                .Include(x => x.Company)
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

        public IQueryable<AppUserRole> UserRoles()
        {
            return _context.UserRoles.AsQueryable();
        }

        public IQueryable<UserFile> UserFiles()
        {
            return _context.UserFiles.AsQueryable();
        }

        public IQueryable<Company> Companies()
        {
            return _context.Companies.AsQueryable();
        }

        public async Task<PaginationDto<AppUser>> GetUserList(GetUserListFilter filter, PaginationCommand command)
        {
            var query = _context.Users
                                .Include(x => x.UserRoles)
                                .ThenInclude(x => x.Role)
                                .Include(x => x.NextOfKin)
                                .Include(x => x.Company)
                                    .ThenInclude(x => x.CompanyContracts.OrderByDescending(y => y.DateCreated).Take(1))
                                .OrderByDescending(x => x.DateCreated)
                                .AsQueryable();

            query = UserQueryHelper.FilterUserList(query, filter);

            return await query.GenerateEntity(command);
        }

    }
}

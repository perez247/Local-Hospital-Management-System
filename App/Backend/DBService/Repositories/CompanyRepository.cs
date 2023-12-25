using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Microsoft.AspNetCore.Identity;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDBContext _context;

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public CompanyRepository(AppDBContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IQueryable<Company> Companies()
        {
            return _context.Companies.AsQueryable();
        }

        public async Task<AppUser> CreateCompany(AppUser newUser, string password)
        {
            var result = await _userManager.CreateAsync(newUser, password);

            if (!result.Succeeded)
            {
                throw new CustomMessageException(result.Errors.FirstOrDefault()?.Description ?? string.Empty);
            }

            return newUser;
        }
    }
}

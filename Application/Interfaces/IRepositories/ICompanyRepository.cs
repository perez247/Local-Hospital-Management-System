using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface ICompanyRepository
    {
        IQueryable<Company> Companies();
        Task<AppUser> CreateCompany(AppUser newUser, string password);
    }
}

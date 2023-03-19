using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities.QueryHelpers
{
    public static class CompanyHelper
    {
        public static async Task<Company> GetHomeCompany(ICompanyRepository companyRepository)
        {
            // Get our home company
            var homeCompany = await companyRepository.Companies()
                                                .FirstOrDefaultAsync(x => x.HomeCompany);
            if (homeCompany == null)
            {
                throw new CustomMessageException("Home company not found, kindly create one");
            }

            return homeCompany;
        }
    }
}

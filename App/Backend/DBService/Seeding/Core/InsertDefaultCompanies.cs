using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Constants;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Seeding.Core
{
    public static class InsertDefaultCompanies
    {
        public async static Task CreateCompanies(AppDBContext context, UserManager<AppUser> userManager)
        {
            await CreateHomeCompany(context, userManager);
            await CreateIndividualCompany(context, userManager);
        }

        private async static Task CreateHomeCompany(AppDBContext context, UserManager<AppUser> userManager)
        {
            var homeCompany = await context.Companies.FirstOrDefaultAsync(x => x.HomeCompany);

            if (homeCompany != null)
            {
                return;
            }

            var newHomeCompany = new AppUser
            {
                Id = Guid.NewGuid(),
                Email = "home@company.com",
                UserName = "home@company.com",
                FirstName = "Home Company",
                LastName = "",
                OtherName = "",
                Address = "123 Home Company street",
                EmailConfirmed = true,
                Company = new Company
                {
                    Id = Guid.NewGuid(),
                    UniqueId = "1234567890",
                    OtherId = "1234567890",
                    ForIndividual = false,
                    HomeCompany = true,
                }
            };

            var result = await userManager.CreateAsync(newHomeCompany, "Abcde@12345");
            await userManager.AddToRolesAsync(newHomeCompany, new List<string>() {
                        ApplicationRoles.Company
                    });
        }

        private async static Task CreateIndividualCompany(AppDBContext context, UserManager<AppUser> userManager)
        {
            var individualCompany = await context.Companies.FirstOrDefaultAsync(x => x.ForIndividual);

            if (individualCompany != null)
            {
                return;
            }

            var newIndividualCompany = new AppUser
            {
                Id = Guid.NewGuid(),
                Email = "individual@company.com",
                UserName = "individual@company.com",
                FirstName = "Individual Company",
                LastName = "",
                OtherName = "",
                Address = "123 Individual Company street",
                EmailConfirmed = true,
                Company = new Company
                {
                    Id = Guid.NewGuid(),
                    UniqueId = "1234567890",
                    OtherId = "1234567890",
                    ForIndividual = true,
                    HomeCompany = false,
                }
            };

            var result = await userManager.CreateAsync(newIndividualCompany, "Abcde@12345");
            await userManager.AddToRolesAsync(newIndividualCompany, new List<string>() {
                        ApplicationRoles.Company
                    });
        }
    }
}

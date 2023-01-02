using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Seeding.Development
{
    public static class InsertFakeUsers
    {
        public async static Task CreateStaff(AppDBContext context, UserManager<AppUser> userManager, string initialDir)
        {
            var oneUser = await context.Staff.FirstOrDefaultAsync();

            if (oneUser != null)
                return;

            var userDir = $"{initialDir}/staff.json";

            using (StreamReader jsonData = new StreamReader(Path.Combine(Path.GetFullPath(userDir))))
            {
                var users = JsonConvert.DeserializeObject<List<AppUser>>(jsonData.ReadToEnd());

                foreach (var user in users)
                {
                    var newUser = new AppUser
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        OtherName = user.OtherName,
                        Address = user.Address,
                        EmailConfirmed = true,
                        Staff = new Staff
                        {
                            Id = Guid.NewGuid(),
                            Active = true,
                        }
                    };

                    var result = await userManager.CreateAsync(newUser, "Abcde@12345");
                    await userManager.AddToRolesAsync(newUser, new List<string>() {
                        ApplicationRoles.Admin
                    });
                }
            }

        }

        public async static Task CreateCompany(AppDBContext context, UserManager<AppUser> userManager, string initialDir)
        {
            var oneUser = await context.Companies.FirstOrDefaultAsync();

            if (oneUser != null)
                return;

            var userDir = $"{initialDir}/company.json";

            using (StreamReader jsonData = new StreamReader(Path.Combine(Path.GetFullPath(userDir))))
            {
                var users = JsonConvert.DeserializeObject<List<AppUser>>(jsonData.ReadToEnd());

                foreach (var user in users)
                {
                    var newUser = new AppUser
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        OtherName = user.OtherName,
                        Address = user.Address,
                        EmailConfirmed = true,
                        Company = new Company
                        {
                            Id = Guid.NewGuid(),
                            UniqueId = user.Id.ToString(),
                            OtherId = user.Id.ToString(),
                        }
                    };

                    var result = await userManager.CreateAsync(newUser, "Abcde@12345");
                    await userManager.AddToRolesAsync(newUser, new List<string>() {
                        ApplicationRoles.Company
                    });
                }
            }

        }

        public async static Task CreatePatient(AppDBContext context, UserManager<AppUser> userManager, string initialDir)
        {
            var oneUser = await context.Patients.FirstOrDefaultAsync();

            if (oneUser != null)
                return;

            var userDir = $"{initialDir}/individual_patients.json";

            using (StreamReader jsonData = new StreamReader(Path.Combine(Path.GetFullPath(userDir))))
            {
                var users = JsonConvert.DeserializeObject<List<AppUser>>(jsonData.ReadToEnd());

                foreach (var user in users)
                {
                    var newUser = new AppUser
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        OtherName = user.OtherName,
                        Address = user.Address,
                        EmailConfirmed = true,
                        Patient = new Patient
                        {
                            Id = Guid.NewGuid(),
                        }
                    };

                    var result = await userManager.CreateAsync(newUser, "Abcde@12345");
                    await userManager.AddToRolesAsync(newUser, new List<string>() {
                        ApplicationRoles.Patients
                    });
                }
            }

        }

        public async static Task CreatePatientInCompany(AppDBContext context, UserManager<AppUser> userManager, string initialDir)
        {
            var oneUser = await context.Patients.Where(x => !x.CompanyId.HasValue).Take(1).ToListAsync();

            if (oneUser.Count() > 0)
                return;

            var companyIds = await context.Companies.Select(x => x.Id).ToArrayAsync();

            var random = new Random();

            var userDir = $"{initialDir}/company_patients.json";

            using (StreamReader jsonData = new StreamReader(Path.Combine(Path.GetFullPath(userDir))))
            {
                var users = JsonConvert.DeserializeObject<List<AppUser>>(jsonData.ReadToEnd());

                foreach (var user in users)
                {
                    var index = random.Next(0, companyIds.Count());
                    var newUser = new AppUser
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        OtherName = user.OtherName,
                        Address = user.Address,
                        EmailConfirmed = true,
                        Patient = new Patient
                        {
                            Id = Guid.NewGuid(),
                            CompanyId = companyIds[index],
                        }
                    };

                    var result = await userManager.CreateAsync(newUser, "Abcde@12345");
                    await userManager.AddToRolesAsync(newUser, new List<string>() {
                        ApplicationRoles.Patients
                    });
                }
            }

        }

    }
}

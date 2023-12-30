using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Constants;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Seeding.Core
{
    public static class InsertTheChosenOne
    {
        public async static Task CreateTheChosenOne(AppDBContext context, UserManager<AppUser> userManager)
        {
            var chosenOne = await context.Staff.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.AppUser.Email == "admin@mail.com");

            if (chosenOne != null)
                return;

            var newChosenOne = new AppUser
            {
                Id = Guid.NewGuid(),
                Email = "admin@mail.com",
                UserName = "admin@mail.com",
                FirstName = "John",
                LastName = "Doe",
                OtherName = "Smith",
                Address = "123 Good lane",
                EmailConfirmed = true,
                Staff = new Staff
                {
                    Id = Guid.NewGuid(),
                    Active = true,
                    Position = "Administrator"
                }
            };

            var result = await userManager.CreateAsync(newChosenOne, "Abcde@12345");
            await userManager.AddToRolesAsync(newChosenOne, new List<string>() {
                        ApplicationRoles.Admin
                    });

        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Seeding.Core
{
    public static class InsertRoles
    {
        public async static Task CreateOrUpdate(AppDBContext context, RoleManager<AppRole> _roleManager)
        {
            IdentityResult roleResult;

            var totalRoles = await _roleManager.Roles.CountAsync();

            if (totalRoles > 0 && ApplicationRoles.Roles().Count == totalRoles)
            {
                return;
            }

            foreach (var roleName in ApplicationRoles.Roles())
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    roleResult = await _roleManager.CreateAsync(new AppRole() { Name = roleName });
                }
            }
        }
    }
}

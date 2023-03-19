using Application.Utilities;
using DBService.Seeding.Core;
using DBService.Seeding.Development;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Seeding
{
    public static class BaseSeeding
    {
        public static async Task<IApplicationBuilder> SeedDefualtDataContextDatabase(this IApplicationBuilder app)
        {
            IServiceProvider serviceProvider = app.ApplicationServices.CreateScope().ServiceProvider;
            try
            {
                // Get basic requirements

                // Get the default data context 
                var defaultDataContext = serviceProvider.GetService<AppDBContext>();
                var defaultDataContextRoleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
                var defaultDataContextUserManager = serviceProvider.GetService<UserManager<AppUser>>();

                if (defaultDataContext == null)
                    throw new Exception("Data access context not initiated");

                // Seed Core data
                await CoreSeeding.BeginSeeding(defaultDataContext, defaultDataContextRoleManager, defaultDataContextUserManager);

                // Seed Development Data
                //if (!EnvironmentFunctions.isEnv("Production"))
                //    await DevelopmentSeeding.BeginSeeding(defaultDataContext, defaultDataContextUserManager);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return app;
        }
    }
}

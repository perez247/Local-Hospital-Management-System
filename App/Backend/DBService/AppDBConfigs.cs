using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService
{
    public static class AppDBConfigs
    {
        public static void ConfigureDefaultDataAccessDatabaseConnections(this IServiceCollection services, string connectionString)
        {
            // throw new Exception(typeof(DefaultDataAccessContext).Assembly.FullName);

            services.AddDbContextPool<AppDBContext>(x => { 
                x.UseNpgsql(connectionString, b => b.MigrationsAssembly("ChannelClinic"));

                //if (EnvironemtUtilityFunctions.IsDevelopment())
                //    x.EnableSensitiveDataLogging();
            });

            // Configure identity for the default database
            services.ConfigureDefaultDataAccesstIdentity();
        }

        public static void ConfigureDefaultDataAccesstIdentity(this IServiceCollection services)
        {
            IdentityBuilder builder = services.AddIdentityCore<AppUser>(opts => {
                opts.SignIn.RequireConfirmedEmail = false;
                opts.Lockout.MaxFailedAccessAttempts = 10;
                opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                opts.User.RequireUniqueEmail = true;
                opts.Lockout.AllowedForNewUsers = false;
                opts.Password.RequiredLength = 6;
                opts.Password.RequiredUniqueChars = 0;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireDigit = false;
            });
            // .AddUserValidator<UniqueEmail<User>>();

            builder = new IdentityBuilder(builder.UserType, typeof(AppRole), builder.Services);
            builder.AddEntityFrameworkStores<AppDBContext>();
            builder.AddRoleValidator<RoleValidator<AppRole>>();
            builder.AddRoleManager<RoleManager<AppRole>>();
            builder.AddSignInManager<SignInManager<AppUser>>();
            builder.AddDefaultTokenProviders();

            // Add cokkies to the application the only resason im using this is for lockout attempt
            services.AddAuthentication().AddApplicationCookie();
        }

        public static void EnsureDefaultDataAccessDatabaseAndMigrationsExtension(this IApplicationBuilder app)
        {
            using (var serviceScope = app?.ApplicationServices?.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                var context = serviceScope?.ServiceProvider.GetRequiredService<AppDBContext>();
                context?.Database.MigrateAsync().Wait();
            }
        }

    }
}

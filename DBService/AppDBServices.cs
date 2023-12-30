using Application.Interfaces.IRepositories;
using DBService.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DBService
{
    public static class AppDBServices
    {
        /// <summary>
        /// Method called to register the default data repository
        /// </summary>
        /// <param name="services"></param>
        public static void AddDBRepositoryServices(this IServiceCollection services)
        {
            // Unit of work for default database
            services.AddScoped<IDBRepository, DBRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IFinancialRespository, FinancialRespository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IAppSettingRepository, AppSettingRepository>();
            services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
        }
    }
}

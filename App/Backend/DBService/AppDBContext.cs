using Application.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DBService
{
    public class AppDBContext : IdentityDbContext
    <
        AppUser, AppRole, Guid,
        IdentityUserClaim<Guid>, AppUserRole, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>
    >
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<SalaryPaymentHistory> SalaryPaymentHistories { get; set; }
        public DbSet<StaffContract> StaffContracts { get; set; }
        public DbSet<StaffTimeTable> StaffTimeTables { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientContract> PatientContracts { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyContract> CompanyContracts { get; set; }
        public DbSet<AppCost> AppCosts { get; set; }
        public DbSet<PatientVital> PatientVitals { get; set; }
        public DbSet<AppInventory> AppInventories { get; set; }
        public DbSet<AppInventoryItem> AppInventoryItems { get; set; }
        public DbSet<AppTicket> AppTickets { get; set; }
        public DbSet<TicketInventory> TicketInventories { get; set; }
        public DbSet<AppAppointment> AppAppointments { get; set; }
        public DbSet<FinancialRecord> FinancialRecords { get; set; }
        public DbSet<FinancialRequest> FinancialRequests { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<AdmissionPrescription> AdmissionPrescriptions { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<MonthlyFinanceRecord> MonthlyFinanceRecords { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var minYear = DateTime.MinValue.Year;
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<BaseEntity> entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.DateCreated = entry.Entity.DateCreated.Year == minYear ? DateTime.UtcNow : entry.Entity.DateCreated.ToUniversalTime();
                        entry.Entity.DateModified = entry.Entity.DateCreated.Year == minYear ? DateTime.UtcNow : entry.Entity.DateCreated.ToUniversalTime();
                        break;

                    case EntityState.Modified:
                        //entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.DateModified = entry.Entity.DateModified.Year == minYear ? DateTime.UtcNow : entry.Entity.DateModified.ToUniversalTime();
                        break;
                }
            }

            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityBaseUser> entry in ChangeTracker.Entries<IdentityBaseUser>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.DateCreated = entry.Entity.DateCreated.Year == minYear ? DateTime.UtcNow : entry.Entity.DateCreated.ToUniversalTime();
                        entry.Entity.DateModified = entry.Entity.DateCreated.Year == minYear ? DateTime.UtcNow : entry.Entity.DateCreated.ToUniversalTime();
                        break;

                    case EntityState.Modified:
                        //entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.DateModified = entry.Entity.DateModified.Year == minYear ? DateTime.UtcNow : entry.Entity.DateModified.ToUniversalTime();
                        break;
                }
            }
            int result = -1;

            try
            {
                result = await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                var savingError = "An error occurred while saving the entity changes";

                var innerException = e.InnerException != null && !string.IsNullOrEmpty(e.InnerException.Message) ? e.InnerException.Message : null;

                var messageToDisplay = e.Message.Contains(savingError) && innerException != null ? innerException : e.Message;

                throw new CustomMessageException(messageToDisplay);
            }

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresExtension("pg_trgm");
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);
        }
    }
}

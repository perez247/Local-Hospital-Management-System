using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {

            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<BaseEntity> entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.DateCreated = DateTime.UtcNow;
                        entry.Entity.DateModified = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        //entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.DateModified = DateTime.UtcNow;
                        break;
                }
            }

            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IdentityBaseUser> entry in ChangeTracker.Entries<IdentityBaseUser>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.DateCreated = DateTime.UtcNow;
                        entry.Entity.DateModified = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        //entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.DateModified = DateTime.UtcNow;
                        break;
                }
            }

            int result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);
        }
    }
}

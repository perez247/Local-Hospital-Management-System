using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.DBSchema
{
    public class CompanySchema : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasMany(entity => entity.CompanyContracts)
                .WithOne(entity => entity.Company)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
            
            builder.HasMany(entity => entity.AppAppointments)
                .WithOne(entity => entity.Company)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade);

            builder.HasMany(entity => entity.AppTickets)
                .WithOne(entity => entity.Company)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade);

            builder.HasOne(entity => entity.AppUser)
                .WithOne(entity => entity.Company)
                .HasForeignKey<Company>(entity => entity.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(entity => entity.Description)
                .IsRequired(false)
                .HasMaxLength(2000);

            builder.Property(entity => entity.UniqueId)
                .IsRequired(false)
                .HasMaxLength(2000);

            builder.Property(entity => entity.OtherId)
                .IsRequired(false)
                .HasMaxLength(2000);
        }
    }
}

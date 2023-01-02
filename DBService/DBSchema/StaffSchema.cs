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
    public class StaffSchema : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.HasOne(entity => entity.AppUser)
                .WithOne(entity => entity.Staff)
                .HasForeignKey<Staff>(entity => entity.AppUserId)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade);

            builder.Property(entity => entity.Level)
                .IsRequired()
                .HasMaxLength(225);

            builder.Property(entity => entity.Position)
                .IsRequired()
                .HasMaxLength(225);

            builder.Property(entity => entity.AccountNumber)
                .IsRequired()
                .HasMaxLength(225);

            builder.Property(entity => entity.BankName)
                .IsRequired()
                .HasMaxLength(225);

            builder.Property(entity => entity.BankId)
                .IsRequired()
                .HasMaxLength(225);

            builder.HasMany(entity => entity.SalaryPaymentHistory)
                .WithOne(entity => entity.Staff)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade);

            builder.HasMany(entity => entity.StaffContract)
                .WithOne(entity => entity.Staff)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
        }
    }
}

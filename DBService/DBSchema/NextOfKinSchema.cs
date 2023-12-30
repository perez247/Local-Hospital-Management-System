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
    public class NextOfKinSchema : IEntityTypeConfiguration<NextOfKin>
    {
        public void Configure(EntityTypeBuilder<NextOfKin> builder)
        {
            builder.HasOne(entity => entity.AppUser)
                .WithOne(entity => entity.NextOfKin)
                .HasForeignKey<NextOfKin>(entity => entity.AppUserId)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade);

            builder.Property(entity => entity.FirstName)
                .IsRequired(false)
                .HasMaxLength(250);

            builder.Property(entity => entity.LastName)
                .IsRequired(false)
                .HasMaxLength(250);

            builder.Property(entity => entity.OtherName)
                .IsRequired(false)
                .HasMaxLength(250);

            builder.Property(entity => entity.Phone1)
                .IsRequired(false)
                .HasMaxLength(100);
           
            builder.Property(entity => entity.Phone2)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(entity => entity.Email)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(entity => entity.Address)
                .IsRequired(false)
                .HasMaxLength(5000);

            builder.Property(entity => entity.Profile)
                .IsRequired(false)
                .HasMaxLength(15000);
        }
    }
}

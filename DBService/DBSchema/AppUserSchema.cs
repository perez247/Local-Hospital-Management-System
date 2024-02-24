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
    public class AppUserSchema : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(entity => entity.FirstName)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(entity => entity.LastName)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(entity => entity.OtherName)
                .IsRequired(false)
                .HasMaxLength(250);

            builder.Property(entity => entity.Occupation)
                .IsRequired(false)
                .HasMaxLength(250);

            builder.Property(entity => entity.Gender)
                .IsRequired(false)
                .HasMaxLength(250);

            builder.Property(entity => entity.Address)
                .IsRequired()
                .HasMaxLength(5000);

            builder.Property(entity => entity.Profile)
                .IsRequired(false)
                .HasMaxLength(15000);
        }
    }
}

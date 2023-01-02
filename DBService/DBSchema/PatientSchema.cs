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
    public class PatientSchema : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasOne(entity => entity.AppUser)
                .WithOne(entity => entity.Patient)
                .HasForeignKey<Patient>(entity => entity.AppUserId)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade);

            builder.HasMany(entity => entity.PatientContracts)
                .WithOne(entity => entity.Patient)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade);

            builder.HasOne(entity => entity.Company)
                .WithMany(entity => entity.Patients)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade);

            builder.Property(entity => entity.Allergies)
                .IsRequired(false)
                .HasMaxLength(10000);
        }
    }
}

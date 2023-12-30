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
    public class PatientVitalSchema : IEntityTypeConfiguration<PatientVital>
    {
        public void Configure(EntityTypeBuilder<PatientVital> builder)
        {
            builder.HasOne(entity => entity.Patient)
                .WithMany(entity => entity.PatientVitals)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(entity => entity.Nurse)
                .WithMany(entity => entity.PatientVitals)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.Data)
                .IsRequired(false)
                .HasMaxLength(5000);
        }
    }
}

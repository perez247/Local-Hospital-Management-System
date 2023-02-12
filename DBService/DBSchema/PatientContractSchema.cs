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
    public class PatientContractSchema : IEntityTypeConfiguration<PatientContract>
    {
        public void Configure(EntityTypeBuilder<PatientContract> builder)
        {
            builder.HasOne(entity => entity.Patient)
                .WithMany(entity => entity.PatientContracts)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(entity => entity.AppCost)
                .WithOne(entity => entity.PatientContract)
                .HasForeignKey<PatientContract>(entity => entity.AppCostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.Duration)
                .IsRequired();
        }
    }
}

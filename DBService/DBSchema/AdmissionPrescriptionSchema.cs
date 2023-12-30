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
    public class AdmissionPrescriptionSchema : IEntityTypeConfiguration<AdmissionPrescription>
    {
        public void Configure(EntityTypeBuilder<AdmissionPrescription> builder)
        {
            builder.HasOne(entity => entity.AppTicket)
                .WithMany(entity => entity.AdmissionPrescriptions)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(entity => entity.Doctor)
                .WithMany(entity => entity.AdmissionPrescriptions)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(entity => entity.TicketInventories)
                .WithOne(entity => entity.AdmissionPrescription)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(entity => entity.OverallDescription)
                .HasMaxLength(5000);
        }
    }
}

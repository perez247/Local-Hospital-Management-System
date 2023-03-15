using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.DBSchema
{
    public class TicketInventorySchema : IEntityTypeConfiguration<TicketInventory>
    {
        public void Configure(EntityTypeBuilder<TicketInventory> builder)
        {
            builder.HasOne(entity => entity.AppTicket)
                .WithMany(entity => entity.TicketInventories)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(entity => entity.AppInventory)
                .WithMany(entity => entity.TicketInventories)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(entity => entity.Proof)
                .IsRequired(false)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<string>>(v)
                );

            builder.Property(x => x.StaffObservation)
                .HasMaxLength(5000);

            builder.Property(x => x.Description)
                .HasMaxLength(5000);

            builder.Property(x => x.DepartmentDescription)
                .HasMaxLength(5000);

            builder.Property(x => x.FinanceDescription)
                .HasMaxLength(5000);

            builder.Property(x => x.DoctorsPrescription)
                .HasMaxLength(5000);

            builder.Property(x => x.PrescribedQuantity)
                .HasMaxLength(5000);

            builder.Property(x => x.LabRadiologyTestResult)
                .HasMaxLength(5000);

        }
    }
}

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
    public class SurgeryTicketPersonnelSchema : IEntityTypeConfiguration<SurgeryTicketPersonnel>
    {
        public void Configure(EntityTypeBuilder<SurgeryTicketPersonnel> builder)
        {
            builder.HasOne(entity => entity.TicketInventory)
                .WithMany(entity => entity.SurgeryTicketPersonnels)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(entity => entity.Personnel)
                .WithMany(entity => entity.SurgeryTicketPersonnels)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(entity => entity.SurgeryRole)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(entity => entity.Description)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(entity => entity.SummaryOfSurgery)
                .IsRequired(false)
                .HasMaxLength(5000);
        }
    }
}

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
    public class AppTicketSchema : IEntityTypeConfiguration<AppTicket>
    {
        public void Configure(EntityTypeBuilder<AppTicket> builder)
        {
            builder.HasOne(entity => entity.Appointment)
                .WithMany(entity => entity.Tickets)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(entity => entity.TicketInventories)
                .WithOne(entity => entity.AppTicket)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(entity => entity.OverallDescription)
                .IsRequired(false)
                .HasMaxLength(5000);
        }
    }
}

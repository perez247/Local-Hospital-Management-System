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
    public class AppInventorySchema : IEntityTypeConfiguration<AppInventory>
    {
        public void Configure(EntityTypeBuilder<AppInventory> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(1000);

            builder.HasMany(entity => entity.AppInventoryItems)
                .WithOne(entity => entity.AppInventory)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(entity => entity.TicketInventories)
                .WithOne(entity => entity.AppInventory)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

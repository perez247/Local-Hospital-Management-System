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
    public class AppInventoryItemSchema : IEntityTypeConfiguration<AppInventoryItem>
    {
        public void Configure(EntityTypeBuilder<AppInventoryItem> builder)
        {
            builder.HasOne(entity => entity.AppInventory)
                .WithMany(entity => entity.AppInventoryItems)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(entity => entity.Company)
                .WithMany(entity => entity.AppInventoryItems)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

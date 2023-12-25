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
    public class FinancialRequestSchema : IEntityTypeConfiguration<FinancialRequest>
    {
        public void Configure(EntityTypeBuilder<FinancialRequest> builder)
        {
            builder.Property(entity => entity.Description)
                .HasMaxLength(5000);

            builder.HasOne(x => x.FinancialRecord)
                .WithMany(entity => entity.FinancialRequest)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

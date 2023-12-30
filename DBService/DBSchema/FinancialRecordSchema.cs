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
    public class FinancialRecordSchema : IEntityTypeConfiguration<FinancialRecord>
    {
        public void Configure(EntityTypeBuilder<FinancialRecord> builder)
        {
            builder.Property(entity => entity.Description)
                .HasMaxLength(5000);

            builder.HasMany(entity => entity.AppCosts)
                .WithOne(entity => entity.FinancialRecord)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(entity => entity.Payments)
                .IsRequired(false)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<Payment>>(v)
                );
        }
    }
}

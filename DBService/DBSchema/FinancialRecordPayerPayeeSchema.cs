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
    public class FinancialRecordPayerPayeeSchema : IEntityTypeConfiguration<FinancialRecordPayerPayee>
    {
        public void Configure(EntityTypeBuilder<FinancialRecordPayerPayee> builder)
        {
            builder.HasOne(entity => entity.AppUser)
                .WithMany(entity => entity.FinancialRecordPayerPayees)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(entity => entity.FinancialRecord)
                .WithMany(entity => entity.FinancialRecordPayerPayees)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(entity => entity.AppCost)
                .WithMany(entity => entity.FinancialRecordPayerPayees)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

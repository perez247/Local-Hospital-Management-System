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
    public class AppCostSchema : IEntityTypeConfiguration<AppCost>
    {
        public void Configure(EntityTypeBuilder<AppCost> builder)
        {
            builder.HasOne(entity => entity.FinancialApprover)
                .WithMany(entity => entity.AppCosts)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(entity => entity.PatientContract)
                .WithOne(entity => entity.AppCost)
                .HasForeignKey<PatientContract>(entity => entity.AppCostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(entity => entity.CompanyContract)
                .WithOne(entity => entity.AppCost)
                .HasForeignKey<CompanyContract>(entity => entity.AppCostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(entity => entity.AppTicket)
                .WithOne(entity => entity.AppCost)
                .HasForeignKey<AppTicket>(entity => entity.AppCostId)
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

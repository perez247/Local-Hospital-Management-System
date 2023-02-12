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
    public class CompanyContractSchema : IEntityTypeConfiguration<CompanyContract>
    {
        public void Configure(EntityTypeBuilder<CompanyContract> builder)
        {
            builder.HasOne(entity => entity.Company)
                .WithMany(entity => entity.CompanyContracts)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(entity => entity.AppCost)
                .WithOne(entity => entity.CompanyContract)
                .HasForeignKey<CompanyContract>(entity => entity.AppCostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.Duration)
                .IsRequired();
        }
    }
}

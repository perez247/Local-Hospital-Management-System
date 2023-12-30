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
    public class AppInventoryDependenciesSchema : IEntityTypeConfiguration<AppInventoryDependencies>
    {
        public void Configure(EntityTypeBuilder<AppInventoryDependencies> builder)
        {
            builder.HasOne(entity => entity.Dependant)
                .WithMany(entity => entity.Dependencies)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

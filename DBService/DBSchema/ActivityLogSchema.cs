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
    public class ActivityLogSchema : IEntityTypeConfiguration<ActivityLog>
    {
        public void Configure(EntityTypeBuilder<ActivityLog> builder)
        {
            builder.Property(prop => prop.ObjectType)
                   .IsRequired(false)
                   .HasMaxLength(255);

            builder.Property(prop => prop.ObjectId)
                   .IsRequired(false)
                   .HasMaxLength(255);

            builder.Property(prop => prop.ActionType)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(prop => prop.ActionDescription)
                   .IsRequired(false);

            builder.Property(prop => prop.OtherDescription)
                   .IsRequired(false);
        }
    }
}

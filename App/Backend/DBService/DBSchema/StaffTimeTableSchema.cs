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
    public class StaffTimeTableSchema : IEntityTypeConfiguration<StaffTimeTable>
    {
        public void Configure(EntityTypeBuilder<StaffTimeTable> builder)
        {
            builder.HasOne(entity => entity.Staff)
                .WithMany(entity => entity.TimeTable)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
        }
    }
}

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
    public class AppAppointmentSchema : IEntityTypeConfiguration<AppAppointment>
    {
        public void Configure(EntityTypeBuilder<AppAppointment> builder)
        {
            builder.HasOne(entity => entity.Patient)
                .WithMany(entity => entity.AppAppointments)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(entity => entity.Doctor)
                .WithMany(entity => entity.AppAppointments)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(entity => entity.Company)
                .WithMany(entity => entity.AppAppointments)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

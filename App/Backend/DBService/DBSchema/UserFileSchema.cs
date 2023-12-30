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
    public class UserFileSchema : IEntityTypeConfiguration<UserFile>
    {
        public void Configure(EntityTypeBuilder<UserFile> builder)
        {
            builder.Property(entity => entity.Base64String)
                .IsRequired();

            builder.HasOne(entity => entity.AppUser)
                .WithMany(entity => entity.UserFiles)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

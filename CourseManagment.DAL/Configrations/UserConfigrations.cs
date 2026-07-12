using CourseManagment.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Configrations
{
    public class UserConfigrations
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Phone)
                .HasMaxLength(20);

            builder.Property(x => x.Address)
                .HasMaxLength(200);

            builder.Property(x => x.Image)
                .HasMaxLength(255);

            builder.Property(x => x.Role)
                .HasMaxLength(20)
                .HasDefaultValue("Student");

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}

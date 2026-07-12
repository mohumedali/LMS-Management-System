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
    public class EnrollmentConfigurations
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Active");

            builder.Property(x => x.EnrollmentDate)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.User)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Course)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.CourseId);

           
            builder.HasIndex(x => new { x.UserId, x.CourseId })
                .IsUnique();
        }
    }
}

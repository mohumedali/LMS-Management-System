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
    public class CourseConfigrations
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Price)
                .HasColumnType("decimal(10,2)");

            builder.Property(x => x.Level)
                .HasMaxLength(30);

            builder.Property(x => x.Language)
                .HasMaxLength(50);

            builder.Property(x => x.Image)
                .HasMaxLength(255);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.CategoryId);

            builder.HasOne(x => x.Instructor)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.InstructorId);
        }

    }
}

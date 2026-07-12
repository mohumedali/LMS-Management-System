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
    public class InstructorConfigrations
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .HasMaxLength(100);

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.Phone)
                .HasMaxLength(20);

            builder.Property(x => x.Specialization)
                .HasMaxLength(100);

            builder.Property(x => x.Image)
                .HasMaxLength(255);

            builder.HasMany(x => x.Courses)
                .WithOne(x => x.Instructor)
                .HasForeignKey(x => x.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

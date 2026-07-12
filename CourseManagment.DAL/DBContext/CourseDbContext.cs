using CourseManagment.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.DBContext
{
    public class CourseDbContext : DbContext
    {
        public CourseDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Payment)
                .WithOne(p => p.Enrollment)
                .HasForeignKey<Payment>(p => p.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>()
                .HasQueryFilter(c => c.IsActive);

            modelBuilder.Entity<Category>()
    .HasQueryFilter(c => c.IsActive);

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CourseDbContext).Assembly);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Instructor> Instructors { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Payment> Payments { get; set; }

    }
}

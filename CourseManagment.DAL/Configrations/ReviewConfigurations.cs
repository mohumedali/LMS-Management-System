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
    public class ReviewConfigurations
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Comment)
                .HasMaxLength(500);

            builder.Property(x => x.ReviewDate)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.User)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Course)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.CourseId);

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint(
                    "CK_Review_Rating",
                    "[Rating] >= 1 AND [Rating] <= 5");
            });

        }
    }
}

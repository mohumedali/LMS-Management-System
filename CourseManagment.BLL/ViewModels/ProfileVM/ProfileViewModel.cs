using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.ProfileVm
{
    public class ProfileViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        // Statistics

        public int TotalEnrollments { get; set; }

        public int CompletedCourses { get; set; }

        public int ActiveCourses { get; set; }

        // Recent Courses

        public IEnumerable<RecentCourseVm> RecentCourses { get; set; }
            = Enumerable.Empty<RecentCourseVm>();
    }
}
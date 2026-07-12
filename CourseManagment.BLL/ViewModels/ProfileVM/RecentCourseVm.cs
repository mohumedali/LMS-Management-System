using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.ProfileVm
{
    public class RecentCourseVm
    {
        public int CourseId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public DateTime EnrollmentDate { get; set; }
    }
}
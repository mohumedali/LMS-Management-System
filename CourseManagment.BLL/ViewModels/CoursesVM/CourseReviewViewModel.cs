using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.CoursesVM
{
    public class CourseReviewViewModel
    {
        public string StudentName { get; set; } = null!;

        public int Rating { get; set; }

        public string Comment { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}

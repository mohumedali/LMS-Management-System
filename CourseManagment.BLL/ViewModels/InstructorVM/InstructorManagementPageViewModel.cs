using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.InstructorsVM
{
    public class InstructorManagementPageViewModel
    {
        public IEnumerable<GetInstructorViewModel> Instructors { get; set; } = [];

        public string? Search { get; set; }

        // Statistics
        public int TotalInstructors { get; set; }

        public int TotalCourses { get; set; }

        public double AverageCoursesPerInstructor { get; set; }
    }
}

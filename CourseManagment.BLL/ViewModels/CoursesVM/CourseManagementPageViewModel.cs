using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.CoursesVM
{
    public class CourseManagementPageViewModel
    {
        public IEnumerable<GetCourseViewModel> Courses { get; set; } = [];

        public List<DropdownItemViewModel> Categories { get; set; } = [];

        public List<DropdownItemViewModel> Instructors { get; set; } = [];

        public string? Search { get; set; }

        public int? CategoryId { get; set; }

        public int? InstructorId { get; set; }
    }
}

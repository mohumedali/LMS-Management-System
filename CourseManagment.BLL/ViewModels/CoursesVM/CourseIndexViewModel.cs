using CourseManagment.BLL.ViewModels.CategoryVM;
using CourseManagment.DAL.Common;
using CourseManagment.BLL.ViewModels.InstructorVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.CoursesVM
{
    public class CourseIndexViewModel
    {
        public CourseFilterViewModel? Filter { get; set; } = new();

        public PaginatedResult<GetCourseViewModel>? Courses { get; set; } = default!;

        public IEnumerable<CategoryDropDownViewModel>? Categories { get; set; } = [];

        public IEnumerable<InstructorDropDownViewModel>? Instructors { get; set; } = [];
    }
}

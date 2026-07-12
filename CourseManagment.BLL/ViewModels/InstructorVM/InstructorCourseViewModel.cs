using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.InstructorsVM
{
    public class InstructorCourseViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = default!;

        public string Category { get; set; } = default!;

        public decimal Price { get; set; }

        public int EnrollmentsCount { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.InstructorsVM
{
    public class GetInstructorViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string Phone { get; set; } = default!;

        public string Specialization { get; set; } = default!;

        public string Image { get; set; } = default!;

        public int CoursesCount { get; set; }
    }
}

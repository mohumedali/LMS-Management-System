using System;
using System.Collections.Generic;

namespace CourseManagment.BLL.ViewModels.InstructorsVM
{
    public class InstructorDetailsViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string Phone { get; set; } = default!;

        public string Specialization { get; set; } = default!;

        public string Bio { get; set; } = default!;

        public string Image { get; set; } = default!;

        public List<InstructorCourseViewModel> Courses { get; set; } = [];
    }
}
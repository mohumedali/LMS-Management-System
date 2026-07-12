using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CourseManagment.DAL.Enums;

namespace CourseManagment.BLL.ViewModels.EnrollmentsVM
{
    public class GetEnrollmentViewModel
    {
        public int Id { get; set; }

        public string StudentName { get; set; } = default!;

        public string? StudentImage { get; set; }

        public string CourseTitle { get; set; } = default!;

        public string InstructorName { get; set; } = default!;

        public DateTime EnrollmentDate { get; set; }

        public EnrollmentStatus Status { get; set; }

        public int Progress { get; set; }
    }
}
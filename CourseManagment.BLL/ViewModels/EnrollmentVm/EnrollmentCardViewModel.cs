using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.EnrollmentVm
{
    public class EnrollmentCardViewModel
    {
        public int EnrollmentId { get; set; }

        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public string CourseImage { get; set; } = string.Empty;

        public string InstructorName { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public DateTime EnrollmentDate { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}

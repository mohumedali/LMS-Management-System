using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CourseManagment.DAL.Enums;

namespace CourseManagment.BLL.ViewModels.EnrollmentsVM
{
    public class EnrollmentManagementPageViewModel
    {
        public IEnumerable<GetEnrollmentViewModel> Enrollments { get; set; } = [];

        public string? Search { get; set; }

        public EnrollmentStatus? Status { get; set; }

        // Statistics

        public int TotalEnrollments { get; set; }

        public int ActiveEnrollments { get; set; }

        public int CompletedEnrollments { get; set; }

        public int CancelledEnrollments { get; set; }
    }
}
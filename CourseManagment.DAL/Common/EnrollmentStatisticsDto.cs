using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Common
{
    public class EnrollmentStatisticsDto
    {
        public int TotalEnrollments { get; set; }

        public int ActiveEnrollments { get; set; }

        public int CompletedEnrollments { get; set; }

        public int CancelledEnrollments { get; set; }
    }
}
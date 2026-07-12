using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CourseManagment.DAL.Enums;

namespace CourseManagment.DAL.Common
{
    public class EnrollmentStatusStatistics
    {
        public int Active { get; set; }

        public int Completed { get; set; }

        public int Cancelled { get; set; }
    }
}

using CourseManagment.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CourseId { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public EnrollmentStatus Status { get; set; }

        public int Progress { get; set; }

        // Navigation Properties
        public User User { get; set; }

        public Course Course { get; set; }

        public Payment? Payment { get; set; }
    }
}

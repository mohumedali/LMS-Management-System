using CourseManagment.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.EnrollmentVm
{
    public class EnrollmentDetailsViewModel
    {
        public int EnrollmentId { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public EnrollmentStatus Status { get; set; }

        // Student

        public int UserId { get; set; }

        public string StudentName { get; set; } = string.Empty;

        public string StudentEmail { get; set; } = string.Empty;

        public string? StudentImage { get; set; }
        public int progress { get; set; }

        // Course

        public int CourseId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Duration { get; set; }

        public string Language { get; set; } = string.Empty;

        public string Level { get; set; } = string.Empty;

        // Category

        public string Category { get; set; } = string.Empty;

        // Instructor

        public string InstructorName { get; set; } = string.Empty;

        public string InstructorImage { get; set; } = string.Empty;

        public string InstructorBio { get; set; } = string.Empty;
    }
}

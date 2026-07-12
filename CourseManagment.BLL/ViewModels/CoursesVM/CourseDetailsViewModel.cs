using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.CoursesVM
{
    public class CourseDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public int Duration { get; set; }

        public string ImageUrl { get; set; } = null!;

        public string Language { get; set; } = null!;

        public string Level { get; set; } = null!;

        public string Category { get; set; } = null!;
        public int StudentsCount { get; set; }

        // Instructor

        public int InstructorId { get; set; }

        public string InstructorName { get; set; } = null!;

        public string? InstructorImage { get; set; }

        public string? InstructorBio { get; set; }

        // Reviews

        public double AverageRating { get; set; }

        public int ReviewsCount { get; set; }

        public List<CourseReviewViewModel> Reviews { get; set; } = [];

        // Lessons

        public List<CourseLessonViewModel> Lessons { get; set; } = [];
    }
}

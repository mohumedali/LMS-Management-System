using CourseManagment.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Duration { get; set; }

        public CourseLevel Level { get; set; }

        public string Language { get; set; }

        public string Image { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        // Foreign Keys
        public int CategoryId { get; set; }

        public int InstructorId { get; set; }

        // Navigation Properties
        public Category Category { get; set; }

        public Instructor Instructor { get; set; }

        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}

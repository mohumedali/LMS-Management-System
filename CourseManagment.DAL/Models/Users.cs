using CourseManagment.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Image { get; set; }

        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation Property
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}

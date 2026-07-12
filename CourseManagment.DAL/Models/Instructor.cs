using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Models
{
    public class Instructor
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Specialization { get; set; }

        public string Bio { get; set; }

        public string Image { get; set; }

        // Navigation Property
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image {  get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Property
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}

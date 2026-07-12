using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Models
{
    public class Lesson
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string VideoUrl { get; set; }

        public int Duration { get; set; }

        // Foreign Key
        public int CourseId { get; set; }

        // Navigation Property
        public Course Course { get; set; }
    }
}

using CourseManagment.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.CoursesVM
{
    public class GetCourseViewModel
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Duration { get; set; }

        public string Level { get; set; }

        public string Language { get; set; }

        public string ImageUrl { get; set; }

        public bool IsActive { get; set; }

        public string CategoryName { get; set; }

        public string InstructorName { get; set; }
        public int EnrollmentsCount { get; set; }
    }
}

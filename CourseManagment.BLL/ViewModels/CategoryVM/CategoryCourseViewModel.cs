using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.CategoriesVM
{
    public class CategoryCourseViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = default!;

        public string InstructorName { get; set; } = default!;

        public decimal Price { get; set; }

        public int StudentsCount { get; set; }
    }
}
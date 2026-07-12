using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.CategoriesVM
{
    public class CategoryDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public int CoursesCount { get; set; }
        public string Image { get; set; } = default!;


        public IEnumerable<CategoryCourseViewModel> Courses { get; set; }
            = [];
    }
}
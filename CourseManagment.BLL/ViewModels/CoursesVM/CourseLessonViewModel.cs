using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.CoursesVM
{
    public class CourseLessonViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string VideoUrl { get; set; }

        public int Duration { get; set; }
    }
}

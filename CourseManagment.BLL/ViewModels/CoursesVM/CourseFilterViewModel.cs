using CourseManagment.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.CoursesVM
{
    public class CourseFilterViewModel
    {
        public string? Search { get; set; }

        public List<int> CategoryIds { get; set; } = new();

        public List<int> InstructorIds { get; set; } = new();

        public List<CourseLevel> Levels { get; set; } = new();

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public double? MinRating { get; set; }

        public string? SortBy { get; set; } = "newest";

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 9;
    }
}

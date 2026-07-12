using CourseManagment.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Common
{
    public class CourseFilter
    {
        public string? Search { get; set; }

        public List<int>? CategoryIds { get; set; }

        public List<int>? InstructorIds { get; set; }

        public List<CourseLevel>? Levels { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public double? MinRating { get; set; }

        public string? SortBy { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 9;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.HomeVm
{
    public class CourseCardVm
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string InstructorName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;


        public string InstructorImage { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;
        public int CategoryId { get; set; } 


        public string ImageUrl { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        public double Rating { get; set; }

        public int StudentsCount { get; set; }

        public int LessonsCount { get; set; }

        public string Level { get; set; } = string.Empty;

        public bool IsBestSeller { get; set; }

        public bool IsHighestRated { get; set; }

        public int DiscountPercentage { get; set; }
    }
}
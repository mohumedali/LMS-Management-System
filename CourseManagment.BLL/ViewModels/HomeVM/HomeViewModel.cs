using CourseManagment.BLL.ViewModels.HomeVm;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.HomeVM
{
        public class HomePageVm
        {
            public IEnumerable<CategoryCardVm> Categories { get; set; }
                = Enumerable.Empty<CategoryCardVm>();

            public IEnumerable<CourseCardVm> PopularCourses { get; set; }
                = Enumerable.Empty<CourseCardVm>();

            public IEnumerable<CourseCardVm> BestSellingCourses { get; set; }
                = Enumerable.Empty<CourseCardVm>();

            public CounterVm Counter { get; set; } = new();
        }
}


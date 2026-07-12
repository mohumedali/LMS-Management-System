using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.DashboardVM;

public class TopCourseViewModel
{
    public int CourseId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public int EnrollmentsCount { get; set; }

    public decimal Price { get; set; }

    public double Rating { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.ViewModels.DashboardVM;

public class DashboardViewModel
{
    // Statistics Cards

    public int TotalCourses { get; set; }

    public int TotalStudents { get; set; }

    public int TotalInstructors { get; set; }

    public int TotalEnrollments { get; set; }

    public decimal TotalRevenue { get; set; }

    public double AverageRating { get; set; }

    // Tables

    public List<TopCourseViewModel> TopCourses { get; set; } = [];

    public List<RecentEnrollmentViewModel> RecentEnrollments { get; set; } = [];

    public List<string> EnrollmentMonths { get; set; } = [];
    public List<int> EnrollmentCounts { get; set; } = [];

    public int ActiveEnrollments { get; set; }
    public int CompletedEnrollments { get; set; }
    public int CancelledEnrollments { get; set; }
}

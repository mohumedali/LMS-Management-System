using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CourseManagment.DAL.Enums;

namespace CourseManagment.BLL.ViewModels.DashboardVM;

public class RecentEnrollmentViewModel
{
    public int EnrollmentId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public string CourseTitle { get; set; } = string.Empty;

    public DateTime EnrollmentDate { get; set; }

    public EnrollmentStatus Status { get; set; }
}

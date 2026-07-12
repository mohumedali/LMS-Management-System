using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CourseManagment.BLL.ViewModels.DashboardVM;

namespace CourseManagment.BLL.Services;

public interface IDashboardService
{
    Task<DashboardViewModel> GetDashboardAsync(CancellationToken ct);
}

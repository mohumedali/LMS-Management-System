using CourseManagment.BLL.ViewModels.HomeVm;
using CourseManagment.BLL.ViewModels.HomeVM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services
{
    public interface IHomeService
    {
        Task<HomePageVm> GetHomePageAsync(CancellationToken ct);

        Task<IEnumerable<CourseCardVm>> GetAllCoursesAsync(CancellationToken ct);

        Task<IEnumerable<CourseCardVm>> GetCoursesByCategoryAsync(
            int categoryId,
            CancellationToken ct);

        Task<IEnumerable<CourseCardVm>> SearchCoursesAsync(
            string search,
            CancellationToken ct);
    }
}

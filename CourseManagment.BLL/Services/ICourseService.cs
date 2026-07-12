using CourseManagment.DAL.Common;
using CourseManagment.BLL.ViewModels.CoursesVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services
{
    public interface ICourseService
    {
        Task<int> AddCourseAsync(
       CreateCourseViewModel model,
       CancellationToken ct);

        Task<int> UpdateCourseAsync(
            int id,
            EditCourseViewModel model,
            CancellationToken ct);

        Task<int> DeleteCourseAsync(
            int id,
            CancellationToken ct);

        Task<GetCourseViewModel?> GetCourseByIdAsync(
            int id,
            CancellationToken ct);

        Task<PaginatedResult<GetCourseViewModel>> GetCoursesAsync(
            CourseFilterViewModel filter,
            CancellationToken ct);

        Task<EditCourseViewModel?> GetCourseForEditAsync(
         int id,
         CancellationToken ct);

        Task<CourseDetailsViewModel?> GetCourseDetailsAsync( int id, CancellationToken ct);

        Task<IEnumerable<GetCourseViewModel>> GetAllCoursesForManagementAsync(CancellationToken ct);
    }
}

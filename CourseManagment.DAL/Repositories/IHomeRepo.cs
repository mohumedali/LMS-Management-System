using CourseManagment.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Repositories
{
    public interface IHomeRepo
    {
        Task<List<Category>> GetCategoriesAsync(CancellationToken ct);

        Task<List<Course>> GetPopularCoursesAsync(CancellationToken ct);

        Task<List<Course>> GetBestSellingCoursesAsync(CancellationToken ct);

        Task<int> GetStudentsCountAsync(CancellationToken ct);

        Task<int> GetCoursesCountAsync(CancellationToken ct);

        Task<int> GetInstructorsCountAsync(CancellationToken ct);

        Task<List<Course>> GetAllCoursesAsync(CancellationToken ct);

        Task<List<Course>> GetCoursesByCategoryAsync(
            int categoryId,
            CancellationToken ct);

        Task<List<Course>> SearchCoursesAsync(
            string search,
            CancellationToken ct);
    }
}

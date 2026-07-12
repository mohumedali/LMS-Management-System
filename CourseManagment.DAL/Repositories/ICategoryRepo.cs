using CourseManagment.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Repositories
{
    public interface ICategoryRepo : IGenaricRepo<Category>
    {
        Task<List<Category>> GetAllWithCoursesAsync(CancellationToken ct);

        Task<Category?> GetByIdWithCoursesAsync(int id, CancellationToken ct);

        Task<bool> ExistsAsync(int id, CancellationToken ct);

        Task<List<Course>> GetCoursesIgnoreFiltersAsync(
    int categoryId,
    CancellationToken ct);

        Task<bool> HasCoursesAsync(
    int categoryId,
    CancellationToken ct);

        Task<List<Course>> GetCoursesAsync(
    int categoryId,
    CancellationToken ct);
    }
}

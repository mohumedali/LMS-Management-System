
using CourseManagment.DAL.Common;
using CourseManagment.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Repositories
{
    public interface ICourseRepo : IGenaricRepo<Course>
    {
        Task<IEnumerable<Course>> GetAllWithDetailsAsync(CancellationToken ct);

        Task<Course?> GetByIdWithDetailsAsync(int id, CancellationToken ct);

        Task<PaginatedResult<Course>> GetFilteredCoursesAsync(
            CourseFilter filter,
            CancellationToken ct);

    }
}

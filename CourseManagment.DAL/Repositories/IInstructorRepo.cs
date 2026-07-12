using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CourseManagment.DAL.Models;

namespace CourseManagment.DAL.Repositories
{
    public interface IInstructorRepo : IGenaricRepo<Instructor>
    {
        Task<List<Instructor>> GetAllWithCoursesAsync(CancellationToken ct);

        Task<Instructor?> GetByIdWithCoursesAsync(int id, CancellationToken ct);

        Task<bool> ExistsAsync(int id, CancellationToken ct);
    }
}
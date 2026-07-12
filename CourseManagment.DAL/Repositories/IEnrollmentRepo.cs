using CourseManagment.DAL.Common;
using CourseManagment.DAL.Enums;
using CourseManagment.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Repositories
{
    public interface IEnrollmentRepo : IGenaricRepo<Enrollment>
    {
        Task<bool> IsEnrolledAsync(
            int userId,
            int courseId,
            CancellationToken ct);

        // Finds a Pending or Cancelled enrollment for this user+course,
        // so a repeat checkout attempt reuses the same row instead of
        // creating a duplicate one.
        Task<Enrollment?> GetExistingNonActiveEnrollmentAsync(
            int userId,
            int courseId,
            CancellationToken ct);

        Task<int> EnrollAsync(
            Enrollment enrollment,
            CancellationToken ct);

        Task<IEnumerable<Enrollment>> GetUserEnrollmentsAsync(
            int userId,
            CancellationToken ct);

        Task<Enrollment?> GetEnrollmentDetailsAsync(
            int id,
            CancellationToken ct);

        Task<List<Enrollment>> GetAllWithDetailsAsync(CancellationToken ct);

        Task<Enrollment?> GetByIdWithDetailsAsync(int id, CancellationToken ct);

        Task<List<Enrollment>> SearchAsync(
            string? search,
            EnrollmentStatus? status,
            CancellationToken ct);

        Task<EnrollmentStatisticsDto> GetStatisticsAsync(
    CancellationToken ct);

    }
}

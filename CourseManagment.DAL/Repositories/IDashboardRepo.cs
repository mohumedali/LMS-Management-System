using CourseManagment.DAL.Common;
using CourseManagment.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Repositories;

public interface IDashboardRepo
{
    Task<int> GetCoursesCountAsync(CancellationToken ct);

    Task<int> GetStudentsCountAsync(CancellationToken ct);

    Task<int> GetInstructorsCountAsync(CancellationToken ct);

    Task<int> GetEnrollmentsCountAsync(CancellationToken ct);

    Task<decimal> GetTotalRevenueAsync(CancellationToken ct);

    Task<double> GetAverageRatingAsync(CancellationToken ct);

    Task<List<Course>> GetTopCoursesAsync(CancellationToken ct);

    Task<List<Enrollment>> GetRecentEnrollmentsAsync(CancellationToken ct);
    Task<List<MonthlyEnrollment>> GetMonthlyEnrollmentsAsync(CancellationToken ct);
    Task<EnrollmentStatusStatistics> GetEnrollmentStatusStatisticsAsync(CancellationToken ct);

}

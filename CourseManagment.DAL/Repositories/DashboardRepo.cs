
using CourseManagment.DAL.Common;
using CourseManagment.DAL.DBContext;
using CourseManagment.DAL.Enums;
using CourseManagment.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Repositories;

public class DashboardRepo : IDashboardRepo
{
    private readonly CourseDbContext _context;

    public DashboardRepo(CourseDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetCoursesCountAsync(CancellationToken ct)
    {
        return await _context.Courses.CountAsync(ct);
    }

    public async Task<int> GetStudentsCountAsync(CancellationToken ct)
    {
        return await _context.Users.CountAsync(ct);
    }

    public async Task<int> GetInstructorsCountAsync(CancellationToken ct)
    {
        return await _context.Instructors.CountAsync(ct);
    }

    public async Task<int> GetEnrollmentsCountAsync(CancellationToken ct)
    {
        return await _context.Enrollments.CountAsync(ct);
    }

    public async Task<decimal> GetTotalRevenueAsync(CancellationToken ct)
    {
        return await _context.Enrollments
            .Include(e => e.Course)
            .IgnoreQueryFilters()
            .SumAsync(e => e.Course.Price, ct);
    }

    public async Task<double> GetAverageRatingAsync(CancellationToken ct)
    {
        if (!await _context.Reviews.AnyAsync(ct))
            return 0;

        return await _context.Reviews
            .AverageAsync(r => r.Rating, ct);
    }

    public async Task<List<Course>> GetTopCoursesAsync(CancellationToken ct)
    {
        return await _context.Courses
            .IgnoreQueryFilters()
            .Include(c => c.Enrollments)
            .OrderByDescending(c => c.Enrollments.Count)
            .Take(5)
            .ToListAsync(ct);
    }

    public async Task<List<Enrollment>> GetRecentEnrollmentsAsync(CancellationToken ct)
    {
        return await _context.Enrollments
            .Include(e => e.User)
            .Include(e => e.Course)
            .IgnoreQueryFilters()
            .OrderByDescending(e => e.EnrollmentDate)
            .Take(10)
            .ToListAsync(ct);
    }
    public async Task<List<MonthlyEnrollment>> GetMonthlyEnrollmentsAsync(CancellationToken ct)
    {
        return await _context.Enrollments
            .GroupBy(x => new
            {
                x.EnrollmentDate.Year,
                x.EnrollmentDate.Month
            })
            .OrderBy(x => x.Key.Year)
            .ThenBy(x => x.Key.Month)
            .Select(x => new MonthlyEnrollment
            {
                Month = new DateTime(
                    x.Key.Year,
                    x.Key.Month,
                    1).ToString("MMM"),

                Count = x.Count()
            })
            .ToListAsync(ct);
    }
    public async Task<EnrollmentStatusStatistics> GetEnrollmentStatusStatisticsAsync(CancellationToken ct)
    {
        var statistics = await _context.Enrollments
            .GroupBy(x => x.Status)
            .Select(g => new
            {
                Status = g.Key,
                Count = g.Count()
            })
            .ToListAsync(ct);

        return new EnrollmentStatusStatistics
        {
            Active = statistics
                .FirstOrDefault(x => x.Status == EnrollmentStatus.Active)?.Count ?? 0,

            Completed = statistics
                .FirstOrDefault(x => x.Status == EnrollmentStatus.Completed)?.Count ?? 0,

            Cancelled = statistics
                .FirstOrDefault(x => x.Status == EnrollmentStatus.Cancelled)?.Count ?? 0
        };
    }
}
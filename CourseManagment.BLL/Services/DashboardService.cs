using CourseManagment.BLL.ViewModels.DashboardVM;
using CourseManagment.DAL.Enums;
using CourseManagment.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepo _dashboardRepo;

    public DashboardService(IDashboardRepo dashboardRepo)
    {
        _dashboardRepo = dashboardRepo;
    }

    public async Task<DashboardViewModel> GetDashboardAsync(CancellationToken ct)
    {
        var topCourses = await _dashboardRepo.GetTopCoursesAsync(ct);

        var recentEnrollments = await _dashboardRepo.GetRecentEnrollmentsAsync(ct);

        var monthlyEnrollments = await _dashboardRepo.GetMonthlyEnrollmentsAsync(ct);

        var statistics = await _dashboardRepo.GetEnrollmentStatusStatisticsAsync(ct);

        var vm = new DashboardViewModel
        {
            

            EnrollmentMonths = monthlyEnrollments
                               .Select(x => x.Month)
                               .ToList(),

            EnrollmentCounts = monthlyEnrollments
                               .Select(x => x.Count)
                               .ToList(),

            ActiveEnrollments = statistics.Active,

            CompletedEnrollments = statistics.Completed,

            CancelledEnrollments = statistics.Cancelled,

            TotalCourses = await _dashboardRepo.GetCoursesCountAsync(ct),

            TotalStudents = await _dashboardRepo.GetStudentsCountAsync(ct),

            TotalInstructors = await _dashboardRepo.GetInstructorsCountAsync(ct),

            TotalEnrollments = await _dashboardRepo.GetEnrollmentsCountAsync(ct),

            TotalRevenue = await _dashboardRepo.GetTotalRevenueAsync(ct),

            AverageRating = await _dashboardRepo.GetAverageRatingAsync(ct),

            TopCourses = topCourses.Select(c => new TopCourseViewModel
            {
                CourseId = c.Id,

                Title = c.Title,

                ImageUrl = c.Image,

                Price = c.Price,

                EnrollmentsCount = c.Enrollments.Count,

                Rating = 4.8 
            }).ToList(),

            RecentEnrollments = recentEnrollments.Select(e => new RecentEnrollmentViewModel
            {
                EnrollmentId = e.Id,

                StudentName = e.User.FullName,

                CourseTitle = e.Course.Title,

                EnrollmentDate = e.EnrollmentDate,

                Status = e.Status
            }).ToList()
        };

        return vm;
    }
}

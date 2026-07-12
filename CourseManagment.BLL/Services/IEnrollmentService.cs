using CourseManagment.BLL.ViewModels.EnrollmentsVM;
using CourseManagment.BLL.ViewModels.EnrollmentVm;
using CourseManagment.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services
{
    public interface IEnrollmentService
    {
        //========================================
        // Enroll
        //========================================
        Task<int> EnrollAsync(
            int userId,
            int courseId,
            CancellationToken ct);

        //========================================
        // My Learning
        //========================================
        Task<IEnumerable<EnrollmentCardViewModel>> GetUserEnrollmentsAsync(
            int userId,
            CancellationToken ct);

        //========================================
        // Details
        //========================================
        Task<EnrollmentDetailsViewModel?> GetEnrollmentDetailsAsync(
            int enrollmentId,
            CancellationToken ct);

        //========================================
        // Cancel Enrollment
        //========================================
        Task<int> CancelEnrollmentAsync(
            int enrollmentId,
            CancellationToken ct);

        //========================================
        // Check Enrollment
        //========================================
        Task<bool> IsEnrolledAsync(
            int userId,
            int courseId,
            CancellationToken ct);

        //=========================
        // Admin
        //=========================

        Task<EnrollmentManagementPageViewModel> GetEnrollmentManagementAsync(
            string? search,
            EnrollmentStatus? status,
            CancellationToken ct);

        Task<EnrollmentDetailsViewModel?> GetAdminEnrollmentDetailsAsync(
            int id,
            CancellationToken ct);

        Task<EditEnrollmentViewModel?> GetEnrollmentForEditAsync(
            int id,
            CancellationToken ct);

        Task<int> UpdateEnrollmentAsync(
            int id,
            EditEnrollmentViewModel model,
            CancellationToken ct);

    }
}


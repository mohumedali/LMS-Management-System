using CourseManagment.BLL.ViewModels.EnrollmentsVM;
using CourseManagment.BLL.ViewModels.EnrollmentVm;
using CourseManagment.DAL.Common;
using CourseManagment.DAL.Enums;
using CourseManagment.DAL.Models;
using CourseManagment.DAL.Repositories;

namespace CourseManagment.BLL.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepo _enrollmentRepo;
        private readonly ICourseRepo _courseRepo;

        public EnrollmentService(
            IEnrollmentRepo enrollmentRepo,
            ICourseRepo courseRepo)
        {
            _enrollmentRepo = enrollmentRepo;
            _courseRepo = courseRepo;
        }

        //========================================
        // Enroll
        //========================================
        public async Task<int> EnrollAsync(
        int userId,
        int courseId,
        CancellationToken ct)
        {
            var course = await _courseRepo.GetByIdAsync(courseId, ct);

            if (course == null)
                throw new InvalidOperationException("Course not found.");

            var enrolled =
                await _enrollmentRepo.IsEnrolledAsync(
                    userId,
                    courseId,
                    ct);

            if (enrolled)
                throw new InvalidOperationException("You are already enrolled in this course.");

            var existing =
                await _enrollmentRepo.GetExistingNonActiveEnrollmentAsync(
                    userId,
                    courseId,
                    ct);

            if (existing != null)
            {
                existing.Status = EnrollmentStatus.Pending;
                existing.EnrollmentDate = DateTime.UtcNow;

                await _enrollmentRepo.UpdateAsync(existing);

                return existing.Id;
            }

            var enrollment = new Enrollment
            {
                UserId = userId,
                CourseId = courseId,
                EnrollmentDate = DateTime.UtcNow,
                Status = EnrollmentStatus.Pending,
                Progress = 0
            };

            var enrollmentId =
                await _enrollmentRepo.EnrollAsync(enrollment, ct);

            return enrollmentId;
        }
        //========================================
        // My Learning
        //========================================
        public async Task<IEnumerable<EnrollmentCardViewModel>> GetUserEnrollmentsAsync(
            int userId,
            CancellationToken ct)
        {
            var enrollments =
                await _enrollmentRepo.GetUserEnrollmentsAsync(userId, ct);

            return enrollments.Select(e => new EnrollmentCardViewModel
            {
                EnrollmentId = e.Id,

                CourseId = e.CourseId,

                CourseTitle = e.Course.Title,

                CourseImage = e.Course.Image,

                InstructorName = e.Course.Instructor.FullName,

                CategoryName = e.Course.Category.Name,

                EnrollmentDate = e.EnrollmentDate,

                Status = e.Status.ToString()
            });
        }

        //========================================
        // Details
        //========================================
        public async Task<EnrollmentDetailsViewModel?> GetEnrollmentDetailsAsync(
     int enrollmentId,
     CancellationToken ct)
        {
            var enrollment =
                await _enrollmentRepo.GetEnrollmentDetailsAsync(
                    enrollmentId,
                    ct);

            if (enrollment == null)
                return null;

            return new EnrollmentDetailsViewModel
            {
                EnrollmentId = enrollment.Id,

                EnrollmentDate = enrollment.EnrollmentDate,

                Status = enrollment.Status,

                CourseId = enrollment.CourseId,

                Title = enrollment.Course.Title,

                Description = enrollment.Course.Description,

                ImageUrl = enrollment.Course.Image,

                Price = enrollment.Course.Price,

                Duration = enrollment.Course.Duration,

                Language = enrollment.Course.Language,

                Level = enrollment.Course.Level.ToString(),

                Category = enrollment.Course.Category.Name,

                InstructorName = enrollment.Course.Instructor.FullName,

                InstructorImage = enrollment.Course.Instructor.Image,

                InstructorBio = enrollment.Course.Instructor.Bio
            };
        }

        //========================================
        // Cancel
        //========================================
        public async Task<int> CancelEnrollmentAsync(
      int enrollmentId,
      CancellationToken ct)
        {
            var enrollment =
                await _enrollmentRepo.GetByIdWithDetailsAsync(
                    enrollmentId,
                    ct);

            if (enrollment == null)
                return 0;

            if (enrollment.Payment != null &&
                enrollment.Payment.Status == PaymentStatus.Paid)
            {
                throw new InvalidOperationException(
                    "Paid enrollments cannot be cancelled.");
            }

            enrollment.Status = EnrollmentStatus.Cancelled;

            return await _enrollmentRepo.UpdateAsync(enrollment);
        }

        //========================================
        // Check Enrollment
        //========================================
        public async Task<bool> IsEnrolledAsync(
            int userId,
            int courseId,
            CancellationToken ct)
        {
            return await _enrollmentRepo.IsEnrolledAsync(
                userId,
                courseId,
                ct);
        }




        //========================================
        // MANAGEMENT
        //========================================

        public async Task<EnrollmentManagementPageViewModel>
            GetEnrollmentManagementAsync(
            string? search,
            EnrollmentStatus? status,
            CancellationToken ct)
        {
            var enrollments = await _enrollmentRepo.SearchAsync(search, status, ct);

            var stats = await _enrollmentRepo.GetStatisticsAsync(ct);

            return new EnrollmentManagementPageViewModel
            {
                Search = search,
                Status = status,

                Enrollments = enrollments.Select(MapToViewModel),

                TotalEnrollments = stats.TotalEnrollments,
                ActiveEnrollments = stats.ActiveEnrollments,
                CompletedEnrollments = stats.CompletedEnrollments,
                CancelledEnrollments = stats.CancelledEnrollments
            };
        }
        //========================================
        // DETAILS
        //========================================

        public async Task<EnrollmentDetailsViewModel?>
            GetAdminEnrollmentDetailsAsync(
            int id,
            CancellationToken ct)
        {
            var enrollment =
                await _enrollmentRepo.GetEnrollmentDetailsAsync(id, ct);

            if (enrollment == null)
                return null;

            return new EnrollmentDetailsViewModel
            {
                EnrollmentId = enrollment.Id,

                Level = enrollment.Course.Level.ToString(),

                Language = enrollment.Course.Language.ToString(),


                StudentName = enrollment.User.FullName,
                StudentEmail = enrollment.User.Email,

                Title = enrollment.Course.Title,

                InstructorName =
                    enrollment.Course.Instructor.FullName,

                Category =
                    enrollment.Course.Category.Name,

                Price =
                    enrollment.Course.Price,
                Duration = enrollment.Course.Duration,

                EnrollmentDate =
                    enrollment.EnrollmentDate,

                Status =
                    enrollment.Status,

                progress =
                    enrollment.Progress,
                ImageUrl = enrollment.Course.Image,
                InstructorImage = enrollment.Course.Instructor.Image

            };
        }

        //========================================
        // EDIT
        //========================================

        public async Task<EditEnrollmentViewModel?>
            GetEnrollmentForEditAsync(
            int id,
            CancellationToken ct)
        {
            var enrollment =
                await _enrollmentRepo.GetByIdWithDetailsAsync(id, ct);

            if (enrollment == null)
                return null;

            return new EditEnrollmentViewModel
            {
                Id = enrollment.Id,
                Status = enrollment.Status,
                Progress = enrollment.Progress
            };
        }

        //========================================
        // UPDATE
        //========================================

        public async Task<int> UpdateEnrollmentAsync(
            int id,
            EditEnrollmentViewModel model,
            CancellationToken ct)
        {
            var enrollment =
                await _enrollmentRepo.GetByIdAsync(id, ct);

            if (enrollment == null)
                return 0;

            enrollment.Status = model.Status;
            enrollment.Progress = model.Progress;

            return await _enrollmentRepo.UpdateAsync(enrollment);
        }


        //========================================
        // MAPPER
        //========================================

        private static GetEnrollmentViewModel MapToViewModel(
            Enrollment enrollment)
        {
            return new GetEnrollmentViewModel
            {
                Id = enrollment.Id,

                StudentName =
                    enrollment.User.FullName,

                CourseTitle =
                    enrollment.Course.Title,

                InstructorName =
                    enrollment.Course.Instructor.FullName,

                EnrollmentDate =
                    enrollment.EnrollmentDate,

                Status =
                    enrollment.Status,

                Progress =
                    enrollment.Progress
            };
        }
    }
}
using CourseManagment.BLL.ViewModels.ProfileVm;
using CourseManagment.DAL.Enums;
using CourseManagment.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepo _profileRepo;

        public ProfileService(IProfileRepo profileRepo)
        {
            _profileRepo = profileRepo;
        }

        //=============================
        // GET PROFILE
        //=============================

        public async Task<ProfileViewModel?> GetProfileAsync(
            int userId,
            CancellationToken ct)
        {
            var user = await _profileRepo.GetProfileAsync(userId, ct);

            if (user == null)
                return null;

            return new ProfileViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                ImageUrl = user.Image,
                CreatedAt = user.CreatedAt,

                TotalEnrollments = user.Enrollments.Count,

                CompletedCourses =
                    user.Enrollments.Count(x =>
                        x.Status == EnrollmentStatus.Completed),

                ActiveCourses =
                    user.Enrollments.Count(x =>
                        x.Status == EnrollmentStatus.Active),

                RecentCourses = user.Enrollments
                    .OrderByDescending(x => x.EnrollmentDate)
                    .Take(5)
                    .Select(x => new RecentCourseVm
                    {
                        CourseId = x.CourseId,
                        Title = x.Course.Title,
                        ImageUrl = x.Course.Image,
                        EnrollmentDate = x.EnrollmentDate
                    })
            };
        }

        //=============================
        // GET PROFILE FOR EDIT
        //=============================

        public async Task<EditProfileViewModel?> GetProfileForEditAsync(
            int userId,
            CancellationToken ct)
        {
            var user = await _profileRepo.GetProfileAsync(userId, ct);

            if (user == null)
                return null;

            return new EditProfileViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                ImageUrl = user.Image
            };
        }

        //=============================
        // UPDATE PROFILE
        //=============================

        public async Task<bool> UpdateProfileAsync(EditProfileViewModel model, CancellationToken ct)
        {

            var emailExists = await _profileRepo.EmailExistsAsync(model.Email, model.Id, ct);
            if (emailExists)
                return false; 

            // 2. هات اليوزر Tracked مش AsNoTracking
            var user = await _profileRepo.GetUserForUpdateAsync(model.Id, ct);
            if (user == null)
                return false;

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Phone = model.Phone;

            if (!string.IsNullOrEmpty(model.ImageUrl))
                user.Image = model.ImageUrl;

            return await _profileRepo.UpdateProfileAsync(user, ct);
        }

        //=============================
        // CHANGE PASSWORD
        //=============================

        public async Task<bool> ChangePasswordAsync(
            int userId,
            ChangePasswordViewModel model,
            CancellationToken ct)
        {
            var user = await _profileRepo.GetProfileAsync(userId, ct);

            if (user == null)
                return false;

            if (user.Password != model.CurrentPassword)
                return false;

            return await _profileRepo.ChangePasswordAsync(
                userId,
                model.NewPassword,
                ct);
        }
    }
}

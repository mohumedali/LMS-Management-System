using CourseManagment.BLL.ViewModels.ProfileVm;

namespace CourseManagment.BLL.Services
{
    public interface IProfileService
    {
        Task<ProfileViewModel?> GetProfileAsync(
            int userId,
            CancellationToken ct);

        Task<EditProfileViewModel?> GetProfileForEditAsync(
            int userId,
            CancellationToken ct);

        Task<bool> UpdateProfileAsync(
            EditProfileViewModel model,
            CancellationToken ct);

        Task<bool> ChangePasswordAsync(
            int userId,
            ChangePasswordViewModel model,
            CancellationToken ct);
    }
}
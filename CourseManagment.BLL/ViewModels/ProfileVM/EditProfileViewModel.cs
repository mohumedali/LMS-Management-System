using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CourseManagment.BLL.ViewModels.ProfileVm
{
    public class EditProfileViewModel
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;


        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string Phone { get; set; } = string.Empty;

        // Current image path - kept so it round-trips through the
        // form (hidden field) when the user doesn't upload a new one.
        public string? ImageUrl { get; set; }

        // New profile picture, optional. If the user doesn't pick a
        // file, ImageUrl above stays unchanged.
        [Display(Name = "Profile Image")]
        public IFormFile? ImageFile { get; set; }
    }
}

using CourseManagment.BLL.Services;
using CourseManagment.BLL.ViewModels.ProfileVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseManagment.PL.Controllers
{

    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProfileController(IProfileService profileService, IWebHostEnvironment webHostEnvironment)
        {
            _profileService = profileService;
            _webHostEnvironment = webHostEnvironment;
        }

        private int UserId
        {
            get
            {
                return int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            }
        }

        //=========================================
        // PROFILE
        //=========================================

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var model = await _profileService.GetProfileAsync(userId.Value, ct);

            return View(model);
        }

        //=========================================
        // EDIT
        //=========================================

        [HttpGet]
        public async Task<IActionResult> Edit(
            CancellationToken ct)
        {
            var model =
                await _profileService.GetProfileForEditAsync(
                    UserId,
                    ct);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            EditProfileViewModel model,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                if (model.ImageFile != null)
                {
                    string imageName = Guid.NewGuid().ToString() +
                                       Path.GetExtension(model.ImageFile.FileName);

                    string folder = Path.Combine(
                        _webHostEnvironment.WebRootPath,
                        "images");

                    Directory.CreateDirectory(folder);

                    string filePath = Path.Combine(folder, imageName);

                    using var stream = new FileStream(filePath, FileMode.Create);

                    await model.ImageFile.CopyToAsync(stream, ct);

                    model.ImageUrl = "~/images/" + imageName;
                }

                var result =
                    await _profileService.UpdateProfileAsync(
                        model,
                        ct);

                if (!result)
                {
                    ModelState.AddModelError(
                        "",
                        "Email already exists.");

                    return View(model);
                }

                TempData["Success"] =
                    "Profile updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.InnerException?.Message ?? ex.Message);

                return View(model);
            }
        }

        //=========================================
        // CHANGE PASSWORD
        //=========================================

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(
            ChangePasswordViewModel model,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result =
                await _profileService.ChangePasswordAsync(
                    UserId,
                    model,
                    ct);

            if (!result)
            {
                ModelState.AddModelError(
                    "",
                    "Current password is incorrect.");

                return View(model);
            }

            TempData["Success"] =
                "Password changed successfully.";

            return RedirectToAction(nameof(Index));
        }
        private int? GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(claim, out int userId))
                return userId;

            return null;
        }
    }
}
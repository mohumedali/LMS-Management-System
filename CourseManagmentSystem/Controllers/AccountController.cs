using System.Security.Claims;
using CourseManagment.DAL.Enums;
using CourseManagment.DAL.Models;
using CourseManagment.DAL.Repositories;
using CourseManagment.BLL.ViewModels.ProfileVM;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CourseManagment.BLL.Services;

namespace CourseManagment.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IGenaricRepo<User> _userRepo;
        private readonly IGenaricRepo<Instructor> _instructorRepo;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AccountTokenService _resetTokenService;

        public AccountController(
            IGenaricRepo<User> userRepo,
            IGenaricRepo<Instructor> instructorRepo,
            IPasswordHasher<User> passwordHasher,
            AccountTokenService resetTokenService)
        {
            _userRepo = userRepo;
            _instructorRepo = instructorRepo;
            _passwordHasher = passwordHasher;
            _resetTokenService = resetTokenService;
        }

        // ==================== Register ====================

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool emailExistsInUsers = await _userRepo.AnyAsync(u => u.Email == model.Email, ct);
            if (emailExistsInUsers)
            {
                ModelState.AddModelError(nameof(model.Email), "This email is already registered");
                return View(model);
            }

            if (model.AccountType == RegisterAccountType.Instructor)
            {
                bool emailExistsInInstructors = await _instructorRepo.AnyAsync(i => i.Email == model.Email, ct);
                if (emailExistsInInstructors)
                {
                    ModelState.AddModelError(nameof(model.Email), "This email is already registered as an Instructor");
                    return View(model);
                }
            }

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                Role = model.AccountType == RegisterAccountType.Instructor ? UserRole.Instructor : UserRole.Student,
                CreatedAt = DateTime.UtcNow,
                Phone = model.Phone ?? string.Empty,
                Address = string.Empty,
                Image = string.Empty
            };

            user.Password = _passwordHasher.HashPassword(user, model.Password);

            await _userRepo.AddAsync(user);

            if (model.AccountType == RegisterAccountType.Instructor)
            {
                var instructor = new Instructor
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Phone = model.Phone ?? string.Empty,
                    Specialization = model.Specialization ?? string.Empty,
                    Bio = model.Bio ?? string.Empty,
                    Image = string.Empty
                };

                await _instructorRepo.AddAsync(instructor);
            }

            TempData["SuccessMessage"] = "Your account has been created successfully. Please log in.";
            return RedirectToAction(nameof(Login));
        }

        // ==================== Login ====================

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userRepo.FirstOrDefault(u => u.Email == model.Email, ct);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View(model);
            }

            var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

            if (verifyResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = model.RememberMe });

            if (user.Role == UserRole.Admin)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return RedirectToAction("Index", "Home");
        }

        // ==================== Logout ====================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        // ==================== Forget Password ====================

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View(new ForgetPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userRepo.FirstOrDefault(u => u.Email == model.Email, ct);

            if (user == null)
            {
                ModelState.AddModelError(nameof(model.Email), "No account found with this email address");
                return View(model);
            }

            string token = _resetTokenService.GenerateToken(user);

            return RedirectToAction(nameof(ResetPassword), new { token });
        }

        // ==================== Reset Password ====================

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            if (!_resetTokenService.TryValidateToken(token, out _, out _))
            {
                TempData["ErrorMessage"] = "This link is invalid or has expired. Please request a new one.";
                return RedirectToAction(nameof(ForgetPassword));
            }

            return View(new ResetPasswordViewModel { Token = token });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!_resetTokenService.TryValidateToken(model.Token, out int userId, out string email))
            {
                TempData["ErrorMessage"] = "This link is invalid or has expired. Please request a new one.";
                return RedirectToAction(nameof(ForgetPassword));
            }

            var user = await _userRepo.GetByIdAsync(userId, ct);

            if (user == null || !string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase))
            {
                TempData["ErrorMessage"] = "Something went wrong. Please start again.";
                return RedirectToAction(nameof(ForgetPassword));
            }

            user.Password = _passwordHasher.HashPassword(user, model.Password);
            await _userRepo.UpdateAsync(user);

            TempData["SuccessMessage"] = "Your password has been changed successfully. Please log in with your new password.";
            return RedirectToAction(nameof(Login));
        }
    }
}

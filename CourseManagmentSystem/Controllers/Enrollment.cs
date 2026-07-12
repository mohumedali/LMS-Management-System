using CourseManagment.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace CourseManagmentSystem.Controllers
{
    
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly ICourseService _courseService;

        
        public EnrollmentController(
            IEnrollmentService enrollmentService,
            ICourseService courseService)
        {
            _enrollmentService = enrollmentService;
            _courseService = courseService;
        }

        //========================================
        // CONFIRM ENROLLMENT
        //========================================
        [HttpGet]
        public async Task<IActionResult> Confirm(
            int courseId,
            CancellationToken ct)
        {
            var course = await _courseService.GetCourseDetailsAsync(courseId, ct);

            if (course == null)
                return NotFound();

            return View(course);
        }

        //========================================
        // ENROLL
        //========================================
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(
    int courseId,
    CancellationToken ct)
        {
            var userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            try
            {
                var enrollmentId =
                    await _enrollmentService.EnrollAsync(
                        userId.Value,
                        courseId,
                        ct);

                var course =
                    await _courseService.GetCourseDetailsAsync(
                        courseId,
                        ct);

                return RedirectToAction(
                    "Create",
                    "Payment",
                    new
                    {
                        enrollmentId,
                        amount = course.Price
                    });
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(
                    "Details",
                    "Course",
                    new { id = courseId });
            }
        }

        //========================================
        // MY LEARNING
        //========================================
        [HttpGet]
        public async Task<IActionResult> MyLearning(
            CancellationToken ct)
        {
            int? userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var courses = await _enrollmentService.GetUserEnrollmentsAsync(userId.Value, ct);

            return View(courses);
        }

        //========================================
        // DETAILS
        //========================================
        [HttpGet]
        public async Task<IActionResult> Details(
            int id,
            CancellationToken ct)
        {
            var enrollment =
                await _enrollmentService.GetEnrollmentDetailsAsync(id, ct);

            if (enrollment == null)
                return NotFound();

            return View(enrollment);
        }

        //========================================
        // CANCEL ENROLLMENT
        //========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(
            int id,
            CancellationToken ct)
        {
            var result =
                await _enrollmentService.CancelEnrollmentAsync(id, ct);

            if (result == 0)
            {
                TempData["Error"] =
                    "Enrollment not found.";

                return RedirectToAction(nameof(MyLearning));
            }

            TempData["Success"] =
                "Enrollment cancelled successfully.";

            return RedirectToAction(nameof(MyLearning));
        }

        private int? GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out var id))
                return null;

            return id;
        }
    }
}

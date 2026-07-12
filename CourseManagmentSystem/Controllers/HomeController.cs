using CourseManagment.BLL.Services;
using CourseManagmentSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CourseManagmentSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            return View(await _homeService.GetHomePageAsync(ct));
        }

        public async Task<IActionResult> Courses(CancellationToken ct)
        {
            return View(await _homeService.GetAllCoursesAsync(ct));
        }

        public async Task<IActionResult> Category(int id, CancellationToken ct)
        {
            return View(await _homeService.GetCoursesByCategoryAsync(id, ct));
        }

        public async Task<IActionResult> Search(string? search, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(search))
                return RedirectToAction(nameof(Index));

            return View(await _homeService.SearchCoursesAsync(search, ct));
        }
    }
}


using CourseManagment.BLL.Services;
using CourseManagment.DAL.Common;
using CourseManagment.BLL.ViewModels.CoursesVM;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourseManagmentSystem.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        private readonly IInstructorService _instructorService;

        public CourseController(
            ICourseService courseService,
            ICategoryService categoryService,
            IInstructorService instructorService)
        {
            _courseService = courseService;
            _categoryService = categoryService;
            _instructorService = instructorService;
        }

        //========================================
        // INDEX
        //========================================
        [HttpGet]
        public async Task<IActionResult> Index(
            CourseFilterViewModel filter,
            CancellationToken ct)
        {
            var result = await _courseService.GetCoursesAsync(filter, ct);

            var vm =  new CourseIndexViewModel
            {
                Filter = filter,
                Courses = result,
                Categories = await _categoryService.GetAllAsync(ct),
                Instructors = await _instructorService.GetAllAsync(ct)
            };

            return View(vm);
        }

        //========================================
        // DETAILS
        //========================================
        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var course =
                await _courseService.GetCourseDetailsAsync(id, ct);

            if (course == null)
                return NotFound();

            return View(course);
        }



      

      
        //========================================
        // LOAD DROPDOWNS
        //========================================
        private async Task LoadDropDowns(CancellationToken ct)
        {
            var categories = await _categoryService.GetAllAsync(ct);

            var instructors = await _instructorService.GetAllAsync(ct);

            ViewBag.Categories = new SelectList(
                categories,
                "Id",
                "Name");

            ViewBag.Instructors = new SelectList(
                instructors,
                "Id",
                "FullName");
        }
    }
}

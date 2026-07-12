using CourseManagment.BLL.Services;
using CourseManagment.BLL.ViewModels;
using CourseManagment.BLL.ViewModels.CategoriesVM;
using CourseManagment.BLL.ViewModels.CoursesVM;
using CourseManagment.BLL.ViewModels.EnrollmentsVM;
using CourseManagment.BLL.ViewModels.InstructorsVM;
using Microsoft.AspNetCore.Hosting;
using CourseManagment.DAL.Enums;
using CourseManagment.DAL.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace CourseManagmentSystem.Controllers
{

    [Authorize]
    
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        private readonly IInstructorService _instructorService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public DashboardController(IDashboardService dashboardService , ICourseService courseService , IWebHostEnvironment webHostEnvironment , ICategoryService categoryService, IInstructorService instructorService , IEnrollmentService enrollmentService)
        {
            _dashboardService = dashboardService;
            _courseService = courseService;
            _categoryService = categoryService;
            _instructorService = instructorService;
            _enrollmentService = enrollmentService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var model = await _dashboardService.GetDashboardAsync(ct);

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> CourseManagment(CancellationToken ct)
        {
            var vm = new CourseManagementPageViewModel
            {
                Courses = await _courseService.GetAllCoursesForManagementAsync(ct),

                Categories = (await _categoryService.GetAllAsync(ct))
                    .Select(c => new DropdownItemViewModel
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList(),

                Instructors = (await _instructorService.GetAllAsync(ct))
                    .Select(i => new DropdownItemViewModel
                    {
                        Id = i.Id,
                        Name = i.FullName
                    }).ToList()
            };

            return View(vm);
        }

        //========================================
        // CREATE GET
        //========================================
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var model = new CreateCourseViewModel();

            await LoadDropDowns(model, ct);

            return View(model);

        }

        //========================================
        // CREATE POST
        //========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
       CreateCourseViewModel model,
       CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropDowns(model, ct);
                return View(model);
            }

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

                    model.ImageUrl = imageName;
                }

                await _courseService.AddCourseAsync(model, ct);

                TempData["CourseSuccess"] = "Course added successfully.";

                return RedirectToAction(nameof(CourseManagment));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);

                await LoadDropDowns(model, ct);

                return View(model);
            }
        }
        //========================================
        // DELETE
        //========================================
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _courseService.DeleteCourseAsync(id, ct);

            switch (result)
            {
                case -1:
                    TempData["CourseError"] = "Cannot delete this course because students are enrolled.";
                    break;

                case 0:
                    TempData["CourseError"] = "Course not found.";
                    break;

                default:
                    TempData["CourseSuccess"] = "Course deleted successfully.";
                    break;
            }

            return RedirectToAction(nameof(CourseManagment));
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
        // EDIT GET
        //========================================
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var course = await _courseService.GetCourseForEditAsync(id, ct);

            if (course == null)
                return NotFound();

            await LoadDropDowns(course, ct);

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            EditCourseViewModel model,
            CancellationToken ct)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                await LoadDropDowns(model, ct);
                return View(model);
            }

            try
            {
                if (model.ImageFile != null)
                {
                    // حذف الصورة القديمة
                    if (!string.IsNullOrEmpty(model.ImageUrl))
                    {
                        string oldPath = Path.Combine(
                            _webHostEnvironment.WebRootPath,
                            "Images",
                            model.ImageUrl);

                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    // حفظ الصورة الجديدة
                    string imageName = Guid.NewGuid().ToString() +
                                       Path.GetExtension(model.ImageFile.FileName);

                    string folder = Path.Combine(
                        _webHostEnvironment.WebRootPath,
                        "Images");

                    Directory.CreateDirectory(folder);

                    string filePath = Path.Combine(folder, imageName);

                    using var stream = new FileStream(filePath, FileMode.Create);

                    await model.ImageFile.CopyToAsync(stream, ct);

                    model.ImageUrl = imageName;
                }

                var result = await _courseService.UpdateCourseAsync(id, model, ct);

                if (result == 0)
                    return NotFound();

                TempData["CourseSuccess"] = "Course updated successfully.";

                return RedirectToAction(nameof(CourseManagment));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                await LoadDropDowns(model, ct);

                return View(model);
            }
        }
        //=================================================
        //  Cateeegoooryyyyyyy
        //================================================



        //========================================
        // CATEGORY MANAGEMENT
        //========================================
        [HttpGet]
        public async Task<IActionResult> CategoryManagement(CancellationToken ct)
        {
            var vm = new CategoryManagementPageViewModel
            {
                Categories = await _categoryService
                    .GetAllCategoriesForManagementAsync(ct)
            };

            return View(vm);
        }

        //========================================
        // CREATE GET
        //========================================
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        //========================================
        // CREATE POST
        //========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(
            CreateCategoryViewModel model,
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
                        "Images");

                    Directory.CreateDirectory(folder);

                    string filePath = Path.Combine(folder, imageName);

                    using var stream = new FileStream(filePath, FileMode.Create);

                    await model.ImageFile.CopyToAsync(stream, ct);

                    model.Image = imageName;
                }

                await _categoryService.AddCategoryAsync(model, ct);

                TempData["CategorySuccess"] = "Category created successfully.";

                return RedirectToAction(nameof(CategoryManagement));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(model);
            }
        }
        //========================================
        // DETAILS
        //========================================
        [HttpGet]
        public async Task<IActionResult> CategoryDetails(
            int id,
            CancellationToken ct)
        {
            var category = await _categoryService
                .GetCategoryDetailsAsync(id, ct);

            if (category == null)
                return NotFound();

            return View(category);
        }

        //========================================
        // EDIT GET
        //========================================
        [HttpGet]
        public async Task<IActionResult> CategoryEdit(
            int id,
            CancellationToken ct)
        {
            var category = await _categoryService
                .GetCategoryForEditAsync(id, ct);

            if (category == null)
                return NotFound();

            return View(category);
        }

        //========================================
        // EDIT POST
        //========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryEdit(
            int id,
            EditCategoryViewModel model,
            CancellationToken ct)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                if (model.ImageFile != null)
                {
                    // حذف الصورة القديمة
                    if (!string.IsNullOrEmpty(model.Image))
                    {
                        string oldPath = Path.Combine(
                            _webHostEnvironment.WebRootPath,
                            "Images",
                            model.Image);

                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    // حفظ الصورة الجديدة
                    string imageName = Guid.NewGuid().ToString() +
                                       Path.GetExtension(model.ImageFile.FileName);

                    string folder = Path.Combine(
                        _webHostEnvironment.WebRootPath,
                        "Images");

                    Directory.CreateDirectory(folder);

                    string filePath = Path.Combine(folder, imageName);

                    using var stream = new FileStream(filePath, FileMode.Create);

                    await model.ImageFile.CopyToAsync(stream, ct);

                    model.Image = imageName;
                }

                var result = await _categoryService
                    .UpdateCategoryAsync(id, model, ct);

                if (result == 0)
                    return NotFound();

                TempData["CategorySuccess"] =
                    "Category updated successfully.";

                return RedirectToAction(nameof(CategoryManagement));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty,
                    ex.InnerException?.Message ?? ex.Message);

                return View(model);
            }
        }

        //========================================
        // DELETE POST
        //========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(
           int id,
           CancellationToken ct)
        {
            try
            {
                var result = await _categoryService.DeleteCategoryAsync(id, ct);

                if (result == 0)
                    return NotFound();

                TempData["CategorySuccess"] = "Category deactivated successfully.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["CategoryError"] = ex.Message;
            }

            return RedirectToAction(nameof(CategoryManagement));
        }







        //========================================
        // LOAD DROPDOWNS
        //========================================
        private async Task LoadDropDowns(CreateCourseViewModel model, CancellationToken ct)
        {
            model.Categories = (await _categoryService.GetAllAsync(ct))
                .Select(x => new DropdownItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

            model.Instructors = (await _instructorService.GetAllAsync(ct))
                .Select(x => new DropdownItemViewModel
                {
                    Id = x.Id,
                    Name = x.FullName
                }).ToList();

        }

        private async Task LoadDropDowns(EditCourseViewModel model , CancellationToken ct)
        {
            model.Categories = (await _categoryService.GetAllAsync(ct))
                .Select(x => new DropdownItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

            model.Instructors = (await _instructorService.GetAllAsync(ct))
                .Select(x => new DropdownItemViewModel
                {
                    Id = x.Id,
                    Name = x.FullName
                }).ToList();
        }

        //========================================
        //INSTRUCTOOOOR
        //======================================= 

        //=====================================================
        // MANAGEMENT
        //=====================================================

        [HttpGet]
        public async Task<IActionResult> InstructorManagement(
            CancellationToken ct)
        {
            var vm = await _instructorService
                .GetAllInstructorsForManagementAsync(ct);

            return View(vm);
        }

        //=====================================================
        // DETAILS
        //=====================================================

        [HttpGet]
        public async Task<IActionResult> DetailsInstructor(
            int id,
            CancellationToken ct)
        {
            var instructor =
                await _instructorService.GetInstructorDetailsAsync(id, ct);

            if (instructor == null)
                return NotFound();

            return View(instructor);
        }

        //=====================================================
        // CREATE GET
        //=====================================================

        [HttpGet]
        public IActionResult CreateInstructor()
        {
            return View();
        }

        //=====================================================
        // CREATE POST
        //=====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInstructor(
    CreateInstructorViewModel model,
    CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.ImageFile != null)
            {
                string imageName = Guid.NewGuid().ToString() +
                                   Path.GetExtension(model.ImageFile.FileName);

                string folder = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    "Images");

                Directory.CreateDirectory(folder);

                string filePath = Path.Combine(folder, imageName);

                using var stream = new FileStream(filePath, FileMode.Create);

                await model.ImageFile.CopyToAsync(stream, ct);

                model.Image = imageName;
            }

            await _instructorService.AddInstructorAsync(model, ct);

            TempData["InstructorSuccess"] =
                "Instructor created successfully.";

            return RedirectToAction(nameof(InstructorManagement));
        }
        //=====================================================
        // EDIT GET
        //=====================================================

        [HttpGet]
        public async Task<IActionResult> EditInstructor(
            int id,
            CancellationToken ct)
        {
            var instructor =
                await _instructorService.GetInstructorForEditAsync(id, ct);

            if (instructor == null)
                return NotFound();

            return View(instructor);
        }

        //=====================================================
        // EDIT POST
        //=====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInstructor(
            int id,
            EditInstructorViewModel model,
            CancellationToken ct)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                if (model.ImageFile != null)
                {
                    // حذف الصورة القديمة
                    if (!string.IsNullOrEmpty(model.Image))
                    {
                        string oldPath = Path.Combine(
                            _webHostEnvironment.WebRootPath,
                            "Images",
                            model.Image);

                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    // حفظ الصورة الجديدة
                    string imageName = Guid.NewGuid().ToString() +
                                       Path.GetExtension(model.ImageFile.FileName);

                    string folder = Path.Combine(
                        _webHostEnvironment.WebRootPath,
                        "Images");

                    Directory.CreateDirectory(folder);

                    string filePath = Path.Combine(folder, imageName);

                    using var stream = new FileStream(filePath, FileMode.Create);

                    await model.ImageFile.CopyToAsync(stream, ct);

                    model.Image = imageName;
                }

                var result = await _instructorService.UpdateInstructorAsync(id,model, ct);

                if (result == 0)
                    return NotFound();

                TempData["InstructorSuccess"] =
                    "Instructor updated successfully.";

                return RedirectToAction(nameof(InstructorManagement));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.InnerException?.Message ?? ex.Message);

                return View(model);
            }
        }

        //=====================================================
        // DELETE POST
        //=====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteInstructor(
            int id,
            CancellationToken ct)
        {
            var result = await _instructorService.DeleteInstructorAsync(id, ct);

            if (result == 0)
                return NotFound();

            if (result == -1)
            {
                TempData["InstructorError"] =
                    "Cannot delete instructor because they have assigned courses.";

                return RedirectToAction(nameof(InstructorManagement));
            }

            TempData["InstructorSuccess"] =
                "Instructor deleted successfully.";

            return RedirectToAction(nameof(InstructorManagement));
        }
        //======================================
        //Enrollmenttttttttttttttt
        //======================================

        //========================================
        // ENROLLMENT MANAGEMENT
        //========================================
        [HttpGet]
        public async Task<IActionResult> EnrollmentManagement(
            string? search,
            EnrollmentStatus? status,
            CancellationToken ct)
        {
            var vm = await _enrollmentService.GetEnrollmentManagementAsync(
                search,
                status,
                ct);

            return View(vm);
        }

        //========================================
        // DETAILS
        //========================================
        [HttpGet]
        public async Task<IActionResult> EnrollmentDetails(
            int id,
            CancellationToken ct)
        {
            var enrollment =
                await _enrollmentService.GetAdminEnrollmentDetailsAsync(
                    id,
                    ct);

            if (enrollment == null)
                return NotFound();

            return View(enrollment);
        }

        //========================================
        // EDIT GET
        //========================================
        [HttpGet]
        public async Task<IActionResult> EditEnrollment(
            int id,
            CancellationToken ct)
        {
            var enrollment =
                await _enrollmentService.GetEnrollmentForEditAsync(
                    id,
                    ct);

            if (enrollment == null)
                return NotFound();

            return View(enrollment);
        }

        //========================================
        // EDIT POST
        //========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEnrollment(
            int id,
            EditEnrollmentViewModel model,
            CancellationToken ct)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result =
                    await _enrollmentService.UpdateEnrollmentAsync(
                        id,
                        model,
                        ct);

                if (result == 0)
                    return NotFound();

                TempData["Success"] =
                    "Enrollment updated successfully.";

                return RedirectToAction(nameof(EnrollmentManagement));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(model);
            }
        }

        //========================================
        // CANCEL ENROLLMENT
        //========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelEnrollment(
            int id,
            CancellationToken ct)
        {
            var result =
                await _enrollmentService.CancelEnrollmentAsync(
                    id,
                    ct);

            if (result == 0)
                return NotFound();

            if (result == -1)
            {
                TempData["CategoryError"] =
                    "Cannot delete category because some courses still have active students.";

                return RedirectToAction(nameof(CategoryManagement));
            }

            TempData["Success"] =
                "Enrollment cancelled successfully.";

            return RedirectToAction(nameof(EnrollmentManagement));
        }
    }
}

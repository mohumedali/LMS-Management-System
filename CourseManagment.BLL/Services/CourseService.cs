using CourseManagment.BLL.ViewModels.CoursesVM;
using CourseManagment.DAL.Common;
using CourseManagment.DAL.Enums;
using CourseManagment.DAL.Models;
using CourseManagment.DAL.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CourseManagment.DAL.Repositories.GenaricRepo;

namespace CourseManagment.BLL.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepo _courseRepo;

        private readonly ICategoryService _categoryService;
        private readonly IInstructorService _instructorService;
        private readonly IEnrollmentRepo _enrollmentRepo;



        public CourseService(ICourseRepo courseRepo , IInstructorService instructorService , ICategoryService categoryService , IEnrollmentRepo enrollmentRepo)
        {
            _courseRepo = courseRepo;
            _instructorService = instructorService;
            _categoryService = categoryService;
            _enrollmentRepo = enrollmentRepo;
        }

        //========================================
        // ADD
        //========================================
        public async Task<int> AddCourseAsync(
            CreateCourseViewModel model,
            CancellationToken ct)
        {
            if (!await _categoryService.ExistsAsync(model.CategoryId, ct))
                throw new InvalidOperationException("Category not found.");

            if (!await _instructorService.ExistsAsync(model.InstructorId, ct))
                throw new InvalidOperationException("Instructor not found.");

            var course = new Course
            {
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                Duration = model.Duration,
                Level = model.Level,
                Language = model.Language,
                Image = model.ImageUrl,
                CategoryId = model.CategoryId,
                InstructorId = model.InstructorId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            return await _courseRepo.AddAsync(course);
        }

        //========================================
        // UPDATE
        //========================================
        public async Task<int> UpdateCourseAsync(
            int id,
            EditCourseViewModel model,
            CancellationToken ct)
        {
            var course = await _courseRepo.GetByIdAsync(id, ct);

            if (course == null)
                return 0;

            if (!await _categoryService.ExistsAsync(model.CategoryId, ct))
                throw new InvalidOperationException("Category not found.");

            if (!await _instructorService.ExistsAsync(model.InstructorId, ct))
                throw new InvalidOperationException("Instructor not found.");

            course.Title = model.Title;
            course.Description = model.Description;
            course.Price = model.Price;
            course.Duration = model.Duration;
            course.Level = model.Level;
            course.Language = model.Language;
            course.Image = model.ImageUrl;
            course.CategoryId = model.CategoryId;
            course.InstructorId = model.InstructorId;

            return await _courseRepo.UpdateAsync(course);
        }

        //========================================
        // DELETE (Soft Delete)
        //========================================
        public async Task<int> DeleteCourseAsync(int id, CancellationToken ct)
        {
            var course = await _courseRepo.GetByIdAsync(id, ct);

            if (course == null)
                return 0;

            var hasStudents = await _enrollmentRepo.AnyAsync(
                x => x.CourseId == id &&
                     x.Status == EnrollmentStatus.Active,
                ct);

            if (hasStudents)
                return -1;

            course.IsActive = false;

            return await _courseRepo.UpdateAsync(course);
        }

        //========================================
        // GET BY ID
        //========================================
        public async Task<GetCourseViewModel?> GetCourseByIdAsync(
            int id,
            CancellationToken ct)
        {
            var course = await _courseRepo.GetByIdWithDetailsAsync(id, ct);

            if (course == null)
                return null;

            return MapToViewModel(course);
        }

        //========================================
        // GET COURSES (Search + Filter + Sort + Pagination)
        //========================================
        public async Task<PaginatedResult<GetCourseViewModel>> GetCoursesAsync(
            CourseFilterViewModel filter,
            CancellationToken ct)
        {
            var query = new CourseFilter
            {
                Search = filter.Search,
                CategoryIds = filter.CategoryIds,
                InstructorIds = filter.InstructorIds,
                Levels = filter.Levels,
                MinPrice = filter.MinPrice,
                MaxPrice = filter.MaxPrice,
                MinRating = filter.MinRating,
                SortBy = filter.SortBy,
                Page = filter.Page,
                PageSize = filter.PageSize
            };

            var result = await _courseRepo.GetFilteredCoursesAsync(query, ct);

            return new PaginatedResult<GetCourseViewModel>
            {
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems,

                Items = result.Items.Select(MapToViewModel)
            };
        }

        //========================================
        // Mapper
        //========================================
        private static GetCourseViewModel MapToViewModel(Course course)
        {
            return new GetCourseViewModel
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                Duration = course.Duration,
                Level = course.Level.ToString(),
                Language = course.Language,
                ImageUrl = course.Image,
                IsActive = course.IsActive,
                CategoryName = course.Category.Name,
                InstructorName = course.Instructor.FullName,
                EnrollmentsCount = course.Enrollments.Count(e => e.Status == EnrollmentStatus.Active)

            };
        }
        //========================================
        // GET COURSE FOR EDIT
        //========================================
        public async Task<EditCourseViewModel?> GetCourseForEditAsync(
            int id,
            CancellationToken ct)
        {
            var course = await _courseRepo.GetByIdAsync(id, ct);

            if (course == null)
                return null;

            return new EditCourseViewModel
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                Duration = course.Duration,
                Level = course.Level,
                Language = course.Language,
                ImageUrl = course.Image,
                CategoryId = course.CategoryId,
                InstructorId = course.InstructorId
            };
        }

        public async Task<CourseDetailsViewModel?> GetCourseDetailsAsync(
    int id,
    CancellationToken ct)
        {
            var course = await _courseRepo.GetByIdWithDetailsAsync(id, ct);

            if (course == null)
                return null;

            return new CourseDetailsViewModel
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                Duration = course.Duration,
                ImageUrl = course.Image,
                Language = course.Language,
                Level = course.Level.ToString(),
                Category = course.Category.Name,

                InstructorId = course.InstructorId,
                InstructorName = course.Instructor.FullName,
                InstructorImage = course.Instructor.Image,
                InstructorBio = course.Instructor.Bio,

                ReviewsCount = course.Reviews.Count,

                AverageRating = course.Reviews.Any()
                    ? course.Reviews.Average(r => r.Rating)
                    : 0,

                Reviews = course.Reviews
                    .OrderByDescending(r => r.ReviewDate)
                    .Select(r => new CourseReviewViewModel
                    {
                        StudentName = r.User.FullName,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        CreatedAt = r.ReviewDate
                    })
                    .ToList(),

                Lessons = course.Lessons.Select(l => new CourseLessonViewModel
                {
                    Id = l.Id,
                    Title = l.Title,
                    Duration = l.Duration,
                    VideoUrl = l.VideoUrl
                })
                    .ToList()
            };
        }
        public async Task<IEnumerable<GetCourseViewModel>> GetAllCoursesForManagementAsync(CancellationToken ct)
        {
            var courses = await _courseRepo.GetAllWithDetailsAsync(ct);

            return courses.Select(MapToViewModel);
        }

    }
}

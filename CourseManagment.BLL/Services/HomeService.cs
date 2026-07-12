using CourseManagment.BLL.ViewModels.HomeVm;
using CourseManagment.BLL.ViewModels.HomeVM;
using CourseManagment.DAL.Models;
using CourseManagment.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services
{
    public class HomeService : IHomeService
    {
        private readonly IHomeRepo _homeRepo;

        public HomeService(IHomeRepo homeRepo)
        {
            _homeRepo = homeRepo;
        }

        public async Task<HomePageVm> GetHomePageAsync(CancellationToken ct)
        {
            var categories = await _homeRepo.GetCategoriesAsync(ct);

            var popularCourses = await _homeRepo.GetPopularCoursesAsync(ct);

            var bestSellingCourses = await _homeRepo.GetBestSellingCoursesAsync(ct);

            return new HomePageVm
            {
                Categories = categories.Select(MapCategory),

                PopularCourses = popularCourses.Select(MapCourse),

                BestSellingCourses = bestSellingCourses.Select(MapCourse),
      
                

                Counter = new CounterVm
                {
                    StudentsCount = await _homeRepo.GetStudentsCountAsync(ct),

                    CoursesCount = await _homeRepo.GetCoursesCountAsync(ct),

                    InstructorsCount = await _homeRepo.GetInstructorsCountAsync(ct)
                }
            };
        }
        public async Task<IEnumerable<CourseCardVm>>
    GetAllCoursesAsync(CancellationToken ct)
        {
            var courses = await _homeRepo.GetAllCoursesAsync(ct);

            return courses.Select(MapCourse);
        }
        public async Task<IEnumerable<CourseCardVm>>
    GetCoursesByCategoryAsync(
    int categoryId,
    CancellationToken ct)
        {
            var courses = await _homeRepo
                .GetCoursesByCategoryAsync(categoryId, ct);

            return courses.Select(MapCourse);
        }

        public async Task<IEnumerable<CourseCardVm>>
    SearchCoursesAsync(
    string search,
    CancellationToken ct)
        {
            var courses = await _homeRepo
                .SearchCoursesAsync(search, ct);

            return courses.Select(MapCourse);
        }



        //========================================

        private static CategoryCardVm MapCategory(Category category)
        {
            return new CategoryCardVm
            {
                Id = category.Id,

                Name = category.Name,

                CoursesCount = category.Courses.Count(c =>c.IsActive),
                ImageUrl = category.Image
                
            };
        }

        //========================================

        private static CourseCardVm MapCourse(Course course)
        {
            return new CourseCardVm
            {
                Id = course.Id,

                Title = course.Title,

                ImageUrl = course.Image,

                Price = course.Price,
                Description = course.Description,

                Level = course.Level.ToString(),

                InstructorName = course.Instructor.FullName,

                CategoryName = course.Category.Name,
                CategoryId = course.Category.Id,

                StudentsCount = course.Enrollments.Count,
            };
        }
    }
}
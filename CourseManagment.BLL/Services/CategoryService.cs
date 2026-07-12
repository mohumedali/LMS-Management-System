using CourseManagment.BLL.ViewModels.CategoriesVM;
using CourseManagment.BLL.ViewModels.CategoryVM;
using CourseManagment.DAL.Enums;
using CourseManagment.DAL.Models;
using CourseManagment.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenaricRepo<Category> _categoryRepo;
        private readonly ICategoryRepo _speccategoryRepo;
        private readonly ICourseRepo _courseRepo;
        private readonly IEnrollmentRepo _enrollmentRepo;




        public CategoryService(IGenaricRepo<Category> categoryRepo , ICategoryRepo speccategoryRepo, IEnrollmentRepo enrollmentRepo, ICourseRepo courseRepo)
        {
            _categoryRepo = categoryRepo;
            _speccategoryRepo = speccategoryRepo;
            _courseRepo = courseRepo;
            _enrollmentRepo = enrollmentRepo;
        }

        public async Task<IEnumerable<CategoryDropDownViewModel>> GetAllAsync(CancellationToken ct)
        {
            var categories = await _categoryRepo.GetAll(ct);

            return categories.Select(c => new CategoryDropDownViewModel
            {
                Id = c.Id,
                Name = c.Name
                
            });
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct)
        {
            return await _categoryRepo.AnyAsync(c => c.Id == id, ct);
        }

        public async Task<List<GetCategoryViewModel>> GetAllCategoriesForManagementAsync(CancellationToken ct)
        {
            var categories = await _speccategoryRepo.GetAllWithCoursesAsync(ct);

            return categories.Select(c => new GetCategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CoursesCount = c.Courses.Count,
                Image = c.Image
            }).ToList();
        }

        public async Task<GetCategoryViewModel?> GetCategoryByIdAsync(
            int id,
            CancellationToken ct)
        {
            var category = await _speccategoryRepo.GetByIdWithCoursesAsync(id, ct);

            if (category == null)
                return null;

            return new GetCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CoursesCount = category.Courses.Count,
                Image = category.Image
                
            };
        }

        public async Task<CategoryDetailsViewModel?> GetCategoryDetailsAsync(
            int id,
            CancellationToken ct)
        {
            var category = await _speccategoryRepo.GetByIdWithCoursesAsync(id, ct);

            if (category == null)
                return null;

            return new CategoryDetailsViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,

                CoursesCount = category.Courses.Count,
                Image = category.Image,

                Courses = category.Courses.Select(c => new CategoryCourseViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    InstructorName = c.Instructor.FullName,
                    Price = c.Price,
                    StudentsCount = c.Enrollments.Count,
                }).ToList()
            };
        }

        public async Task<EditCategoryViewModel?> GetCategoryForEditAsync(
            int id,
            CancellationToken ct)
        {
            var category = await _categoryRepo.GetByIdAsync(id, ct);

            if (category == null)
                return null;

            return new EditCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Image = category.Image,
                
            };
        }

        public async Task<int> AddCategoryAsync(
            CreateCategoryViewModel model,
            CancellationToken ct)
        {
            var category = new Category
            {
                Name = model.Name,
                Description = model.Description,
                Image = model.Image,
                
            };

            return await _categoryRepo.AddAsync(category);
        }

        public async Task<int> UpdateCategoryAsync(
            int id,
            EditCategoryViewModel model,
            CancellationToken ct)
        {
            var category = await _categoryRepo.GetByIdAsync(id, ct);

            if (category == null)
                return 0;

            category.Name = model.Name;
            category.Description = model.Description;
            category.Image = model.Image;

            return await _categoryRepo.UpdateAsync(category);
        }

        public async Task<int> DeleteCategoryAsync(
            int id,
            CancellationToken ct)
        {
            var category = await _categoryRepo.GetByIdAsync(id, ct);

            if (category == null)
                return 0;

            var courses = await _speccategoryRepo.GetCoursesAsync(id, ct);

            if (courses.Any(c => c.IsActive))
                throw new InvalidOperationException(
                    "Cannot deactivate this category because it contains active courses.");

            category.IsActive = false;

            return await _categoryRepo.UpdateAsync(category);
        }
    }
}

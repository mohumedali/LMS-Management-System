using CourseManagment.DAL.DBContext;
using CourseManagment.DAL.Enums;
using CourseManagment.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagment.DAL.Repositories
{
    public class HomeRepo : IHomeRepo
    {
        private readonly CourseDbContext _context;

        public HomeRepo(CourseDbContext context)
        {
            _context = context;
        }


        //========================================
        // Categories
        //========================================

        public async Task<List<Category>> GetCategoriesAsync(
            CancellationToken ct)
        {
            return await _context.Categories
                .Where(x => x.IsActive)
                .Include(x => x.Courses)
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync(ct);
        }


        //========================================
        // Popular Courses
        //========================================

        public async Task<List<Course>> GetPopularCoursesAsync(
            CancellationToken ct)
        {
            return await _context.Courses
                .Where(x => x.IsActive)
                .Include(x => x.Instructor)
                .Include(x => x.Category)
                .Include(x => x.Enrollments)
                .AsNoTracking()
                .OrderByDescending(x => x.Enrollments.Count)
                .Take(8)
                .ToListAsync(ct);
        }


        //========================================
        // Best Selling Courses
        //========================================

        public async Task<List<Course>> GetBestSellingCoursesAsync(
            CancellationToken ct)
        {
            return await _context.Courses
                .Where(x => x.IsActive)
                .Include(x => x.Instructor)
                .Include(x => x.Category)
                .Include(x => x.Enrollments)
                .AsNoTracking()
                .OrderByDescending(x => x.Enrollments.Count)
                .Take(10)
                .ToListAsync(ct);
        }


        //========================================
        // Dashboard Counters
        //========================================

        public async Task<int> GetStudentsCountAsync(
            CancellationToken ct)
        {
            return await _context.Users
                .CountAsync(
                    x => x.Role == UserRole.Student,
                    ct);
        }


        public async Task<int> GetCoursesCountAsync(
            CancellationToken ct)
        {
            return await _context.Courses
                .CountAsync(x => x.IsActive, ct);
        }


        public async Task<int> GetInstructorsCountAsync(
            CancellationToken ct)
        {
            return await _context.Instructors
                .CountAsync(ct);
        }



        //========================================
        // All Courses
        //========================================

        public async Task<List<Course>> GetAllCoursesAsync(
            CancellationToken ct)
        {
            return await _context.Courses
                .Where(x => x.IsActive)
                .Include(x => x.Category)
                .Include(x => x.Instructor)
                .Include(x => x.Enrollments)
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .ToListAsync(ct);
        }



        //========================================
        // Courses By Category
        //========================================

        public async Task<List<Course>> GetCoursesByCategoryAsync(
            int categoryId,
            CancellationToken ct)
        {
            return await _context.Courses
                .Where(x =>
                    x.CategoryId == categoryId &&
                    x.IsActive)
                .Include(x => x.Category)
                .Include(x => x.Instructor)
                .Include(x => x.Enrollments)
                .AsNoTracking()
                .ToListAsync(ct);
        }



        //========================================
        // Search Courses
        //========================================

        public async Task<List<Course>> SearchCoursesAsync(
            string search,
            CancellationToken ct)
        {
            return await _context.Courses
                .Where(x =>
                    x.IsActive &&
                    (
                        x.Title.Contains(search) ||
                        x.Description.Contains(search) ||
                        x.Category.Name.Contains(search) ||
                        x.Instructor.FullName.Contains(search)
                    ))
                .Include(x => x.Category)
                .Include(x => x.Instructor)
                .Include(x => x.Enrollments)
                .AsNoTracking()
                .ToListAsync(ct);
        }

    }
}
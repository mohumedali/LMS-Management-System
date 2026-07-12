using CourseManagment.DAL.DBContext;
using CourseManagment.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Repositories
{
    public class CategoryRepo
        : GenaricRepo.GenaricRepository<Category>, ICategoryRepo
    {
        private readonly CourseDbContext _context;

        public CategoryRepo(CourseDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllWithCoursesAsync(
            CancellationToken ct)
        {
            return await _context.Categories
                .Where(c => c.IsActive)
                .Include(c => c.Courses)
                .ToListAsync(ct);
        }
        public async Task<Category?> GetByIdWithCoursesAsync(
            int id,
            CancellationToken ct)
        {
            return await _context.Categories
                .Where(c => c.IsActive)
                .Include(c => c.Courses)
                    .ThenInclude(c => c.Instructor)
                .Include(c => c.Courses)
                    .ThenInclude(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct)
        {
            return await _context.Categories
.AnyAsync(x => x.Id == id && x.IsActive, ct);
        }

        public async Task<List<Course>> GetCoursesIgnoreFiltersAsync(
    int categoryId,
    CancellationToken ct)
        {
            return await _context.Courses
                .Where(c => c.CategoryId == categoryId
                         && c.IsActive)
                .ToListAsync(ct);
        }

        public async Task<bool> HasCoursesAsync(
            int categoryId,
            CancellationToken ct)
        {
            return await _context.Courses
                .AnyAsync(c =>
                    c.CategoryId == categoryId &&
                    c.IsActive,
                    ct);
        }

        public async Task<List<Course>> GetCoursesAsync(
    int categoryId,
    CancellationToken ct)
        {
            return await _context.Courses
                .Where(c => c.CategoryId == categoryId)
                .ToListAsync(ct);
        }
    }
}

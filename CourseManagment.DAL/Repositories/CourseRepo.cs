using CourseManagment.DAL.DBContext;
using CourseManagment.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CourseManagment.DAL.Repositories.ICourseRepo;
using static CourseManagment.DAL.Repositories.GenaricRepo;
using Microsoft.EntityFrameworkCore;

using CourseManagment.DAL.Common;

namespace CourseManagment.DAL.Repositories
{
        public class CourseRepository : GenaricRepository<Course>, ICourseRepo
        {
            private readonly CourseDbContext _context;

            public CourseRepository(CourseDbContext context)
                : base(context)
            {
                _context = context;
            }


        //==========================================
        // GET ALL WITH DETAILS
        //==========================================
        public async Task<IEnumerable<Course>> GetAllWithDetailsAsync(CancellationToken ct)
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .Where(c => c.IsActive)
                .OrderByDescending(c => c.CreatedAt)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        //==========================================
        // GET BY ID WITH DETAILS
        //==========================================
        public async Task<Course?> GetByIdWithDetailsAsync(int id, CancellationToken ct)
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .Include(c => c.Reviews)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive, ct);
        }

        public async Task<PaginatedResult<Course>> GetFilteredCoursesAsync(
        CourseFilter filter,
        CancellationToken ct)
        {
            IQueryable<Course> query = _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .AsNoTracking()
                .Where(c => c.IsActive);

            //=========================
            // Search
            //=========================

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(c =>
                    c.Title.Contains(filter.Search));
            }

            //=========================
            // Category
            //=========================

            if (filter.CategoryIds != null &&
                filter.CategoryIds.Any())
            {
                query = query.Where(c =>
                    filter.CategoryIds.Contains(c.CategoryId));
            }

            //=========================
            // Instructor
            //=========================

            if (filter.InstructorIds != null &&
                filter.InstructorIds.Any())
            {
                query = query.Where(c =>
                    filter.InstructorIds.Contains(c.InstructorId));
            }

            //=========================
            // Level
            //=========================

            if (filter.Levels != null &&
                filter.Levels.Any())
            {
                query = query.Where(c =>
                    filter.Levels.Contains(c.Level));
            }

            //=========================
            // Price
            //=========================

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(c =>
                    c.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(c =>
                    c.Price <= filter.MaxPrice.Value);
            }

            //=========================
            // Rating
            //=========================

            if (filter.MinRating.HasValue)
            {
                query = query.Where(c =>
                    c.Reviews.Any() &&
                    c.Reviews.Average(r => r.Rating) >= filter.MinRating.Value);
            }

            //=========================
            // Sorting
            //=========================

            query = filter.SortBy switch
            {
                "price" => query.OrderBy(c => c.Price),

                "price_desc" => query.OrderByDescending(c => c.Price),

                "title" => query.OrderBy(c => c.Title),

                "newest" => query.OrderByDescending(c => c.CreatedAt),

                _ => query.OrderByDescending(c => c.CreatedAt)
            };

            //=========================
            // Pagination
            //=========================

            int totalItems = await query.CountAsync(ct);

            var courses = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync(ct);

            return new PaginatedResult<Course>
            {
                Items = courses,
                CurrentPage = filter.Page,
                PageSize = filter.PageSize,
                TotalItems = totalItems
            };
        }

        
    }
}

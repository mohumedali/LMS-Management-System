using CourseManagment.DAL.DBContext;
using CourseManagment.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CourseManagment.DAL.Repositories.GenaricRepo;

namespace CourseManagment.DAL.Repositories
{
    public class InstructorRepo
        : GenaricRepository<Instructor>, IInstructorRepo
    {
        private readonly CourseDbContext _context;

        public InstructorRepo(CourseDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<List<Instructor>> GetAllWithCoursesAsync(CancellationToken ct)
        {
            return await _context.Instructors
                .Include(i => i.Courses)
                .ToListAsync(ct);
        }

        public async Task<Instructor?> GetByIdWithCoursesAsync(int id, CancellationToken ct)
        {
            return await _context.Instructors
                .Include(i => i.Courses)
                    .ThenInclude(c => c.Category)
                .Include(i => i.Courses)
                    .ThenInclude(c => c.Enrollments)
                .FirstOrDefaultAsync(i => i.Id == id, ct);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct)
        {
            return await _context.Instructors
                .AnyAsync(x => x.Id == id, ct);
        }
    }
}
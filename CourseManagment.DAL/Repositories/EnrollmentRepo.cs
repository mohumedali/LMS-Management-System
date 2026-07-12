using CourseManagment.DAL.Common;
using CourseManagment.DAL.DBContext;
using CourseManagment.DAL.Enums;
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
    public class EnrollmentRepo : GenaricRepository<Enrollment>, IEnrollmentRepo
    {
        private readonly CourseDbContext _context;

        public EnrollmentRepo(CourseDbContext context) : base(context)
        {
            _context = context;
        }

        //==========================================
        // CHECK IF USER IS ENROLLED
        //==========================================
        public async Task<bool> IsEnrolledAsync(
            int userId,
            int courseId,
            CancellationToken ct)
        {
            return await _context.Enrollments
                .AnyAsync(e =>
                    e.UserId == userId &&
                    e.CourseId == courseId &&
                    e.Status == EnrollmentStatus.Active,
                    ct);
        }

        //==========================================
        // FIND REUSABLE (PENDING/CANCELLED) ENROLLMENT
        //==========================================
        public async Task<Enrollment?> GetExistingNonActiveEnrollmentAsync(
            int userId,
            int courseId,
            CancellationToken ct)
        {
            return await _context.Enrollments
                .Include(e => e.Payment)
                .Where(e =>
                    e.UserId == userId &&
                    e.CourseId == courseId &&
                    (e.Status == EnrollmentStatus.Pending ||
                     e.Status == EnrollmentStatus.Cancelled))
                .OrderByDescending(e => e.EnrollmentDate)
                .FirstOrDefaultAsync(ct);
        }

        //==========================================
        // ENROLL USER
        //==========================================
        public async Task<int> EnrollAsync(
            Enrollment enrollment,
            CancellationToken ct)
        {
            await _context.Enrollments.AddAsync(enrollment, ct);

            await _context.SaveChangesAsync(ct);

            return enrollment.Id;
        }
        //==========================================
        // GET USER ENROLLMENTS
        //==========================================
        public async Task<IEnumerable<Enrollment>> GetUserEnrollmentsAsync(
            int userId,
            CancellationToken ct)
        {
            return await _context.Enrollments
                .Include(e => e.Course)
                    .ThenInclude(c => c.Category)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Instructor)
                .Where(e =>
                    e.UserId == userId &&
                    e.Status == EnrollmentStatus.Active)
                .OrderByDescending(e => e.EnrollmentDate)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        //==========================================
        // GET ENROLLMENT DETAILS
        //==========================================
        public async Task<Enrollment?> GetEnrollmentDetailsAsync(
            int id,
            CancellationToken ct)
        {
            return await _context.Enrollments
                .Include(e => e.User)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Category)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Instructor)
                .AsNoTracking()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(e => e.Id == id, ct);
        }

        public async Task<List<Enrollment>> GetAllWithDetailsAsync(CancellationToken ct)
        {
            return await _context.Enrollments
                .Include(x => x.User)
                .Include(x => x.Course)
                    .ThenInclude(c => c.Instructor)
                .IgnoreQueryFilters()
                .AsNoTracking()
                .OrderByDescending(x => x.EnrollmentDate)
                .ToListAsync(ct);
        }

        public async Task<Enrollment?> GetByIdWithDetailsAsync(
            int id,
            CancellationToken ct)
        {
            return await _context.Enrollments
                .IgnoreQueryFilters()
                .Include(x => x.User)
                .Include(x => x.Course)
                    .ThenInclude(c => c.Instructor)
                .Include(x => x.Course)
                    .ThenInclude(c => c.Category)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<List<Enrollment>> SearchAsync(
            string? search,
            EnrollmentStatus? status,
            CancellationToken ct)
        {
            IQueryable<Enrollment> query = _context.Enrollments
                .Include(x => x.User)
                .Include(x => x.Course)
                    .ThenInclude(c => c.Instructor)
                .IgnoreQueryFilters();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.User.FullName.Contains(search) ||
                    x.Course.Title.Contains(search));
            }

            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }

            return await query
                .AsNoTracking()
                .OrderByDescending(x => x.EnrollmentDate)
                .ToListAsync(ct);
        }

        public async Task<EnrollmentStatisticsDto> GetStatisticsAsync(
         CancellationToken ct)
        {
            return new EnrollmentStatisticsDto
            {
                TotalEnrollments = await _context.Enrollments.CountAsync(ct),

                ActiveEnrollments = await _context.Enrollments
                    .CountAsync(x => x.Status == EnrollmentStatus.Active, ct),

                CompletedEnrollments = await _context.Enrollments
                    .CountAsync(x => x.Status == EnrollmentStatus.Completed, ct),

                CancelledEnrollments = await _context.Enrollments
                    .CountAsync(x => x.Status == EnrollmentStatus.Cancelled, ct)
            };
        }

    }
}

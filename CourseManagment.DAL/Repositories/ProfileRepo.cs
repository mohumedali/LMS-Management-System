using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CourseManagment.DAL.DBContext;
using CourseManagment.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagment.DAL.Repositories
{
    public class ProfileRepo : IProfileRepo
    {
        private readonly CourseDbContext _context;

        public ProfileRepo(CourseDbContext context)
        {
            _context = context;
        }

        //==========================================
        // GET PROFILE
        //==========================================
        public async Task<User?> GetProfileAsync(
            int userId,
            CancellationToken ct)
        {
            return await _context.Users
                .Include(x => x.Enrollments)
                    .ThenInclude(e => e.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId, ct);
        }

        //==========================================
        // UPDATE PROFILE
        //==========================================
        public async Task<bool> UpdateProfileAsync(
            User user,
            CancellationToken ct)
        {
            return await _context.SaveChangesAsync(ct) > 0;
        }
        //==========================================
        // CHANGE PASSWORD
        //==========================================
        public async Task<bool> ChangePasswordAsync(
            int userId,
            string newPassword,
            CancellationToken ct)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId, ct);

            if (user == null)
                return false;

            user.Password = newPassword;

            return await _context.SaveChangesAsync(ct) > 0;
        }

        //==========================================
        // CHECK EMAIL EXISTS
        //==========================================
        public async Task<bool> EmailExistsAsync(
            string email,
            int userId,
            CancellationToken ct)
        {
            return await _context.Users
                .AnyAsync(u =>
                    u.Email == email &&
                    u.Id != userId,
                    ct);
        }
        public async Task<User?> GetUserForUpdateAsync(int userId, CancellationToken ct)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId, ct);
            // من غير AsNoTracking عشان EF يتابع التغييرات
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CourseManagment.DAL.DBContext;
using CourseManagment.DAL.Interfaces;
using CourseManagment.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagment.DAL.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly CourseDbContext _context;

        public PaymentRepository(CourseDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(
        Payment payment,
        CancellationToken ct)
        {
            await _context.Payments.AddAsync(payment, ct);

            await _context.SaveChangesAsync(ct);

            return payment.Id;
        }
        public async Task UpdateAsync(
            Payment payment,
            CancellationToken ct)
        {
            _context.Payments.Update(payment);

            // BUGFIX: this method used to only mark the entity as
            // Modified without ever saving - so status changes
            // (Paid/Failed) and the related Enrollment.Status update
            // never reached the database.
            await _context.SaveChangesAsync(ct);
        }

        public async Task<Payment?> GetByIdAsync(
            int id,
            CancellationToken ct)
        {
            return await _context.Payments
                .Include(x => x.Enrollment)
                    .ThenInclude(e => e.User)

                .Include(x => x.Enrollment)
                    .ThenInclude(e => e.Course)

                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<Payment?> GetByEnrollmentIdAsync(
            int enrollmentId,
            CancellationToken ct)
        {
            return await _context.Payments
                .Include(x => x.Enrollment)
                    .ThenInclude(e => e.User)

                .Include(x => x.Enrollment)
                    .ThenInclude(e => e.Course)

                .FirstOrDefaultAsync(x =>
                    x.EnrollmentId == enrollmentId,
                    ct);
        }

        public async Task<IEnumerable<Payment>> GetUserPaymentsAsync(
            int userId,
            CancellationToken ct)
        {
            return await _context.Payments

                .Include(x => x.Enrollment)
                    .ThenInclude(e => e.User)

                .Include(x => x.Enrollment)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c.Category)

                .Include(x => x.Enrollment)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c.Instructor)

                .Where(x => x.Enrollment!.UserId == userId)
                .OrderByDescending(x => x.PaymentDate)
                .ToListAsync(ct);
        }

        public async Task SaveChangesAsync(
            CancellationToken ct)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}
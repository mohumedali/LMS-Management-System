using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CourseManagment.DAL.Models;

namespace CourseManagment.DAL.Interfaces
{
    public interface IPaymentRepository
    {
        Task<int> AddAsync(
                Payment payment,
                CancellationToken ct);

        Task UpdateAsync(Payment payment, CancellationToken ct);

        Task<Payment?> GetByIdAsync(int id, CancellationToken ct);

        Task<Payment?> GetByEnrollmentIdAsync(int enrollmentId, CancellationToken ct);

        Task<IEnumerable<Payment>> GetUserPaymentsAsync(int userId, CancellationToken ct);

        Task SaveChangesAsync(CancellationToken ct);
    }
}
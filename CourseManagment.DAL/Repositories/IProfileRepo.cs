using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CourseManagment.DAL.Models;

namespace CourseManagment.DAL.Repositories
{
    public interface IProfileRepo
    {
        Task<User?> GetProfileAsync(
            int userId,
            CancellationToken ct);

        Task<bool> UpdateProfileAsync(
            User user,
            CancellationToken ct);

        Task<bool> ChangePasswordAsync(
            int userId,
            string newPassword,
            CancellationToken ct);

        Task<bool> EmailExistsAsync(
            string email,
            int userId,
            CancellationToken ct);

        Task<User?> GetUserForUpdateAsync(int userId, CancellationToken ct);
    }
}
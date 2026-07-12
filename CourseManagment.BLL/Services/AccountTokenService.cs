using CourseManagment.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using CourseManagment.DAL.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebUtilities;

namespace CourseManagment.BLL.Services
{
    public class AccountTokenService
    {
        private const string Purpose = "Learnio.PasswordResetToken";
        private static readonly TimeSpan TokenLifetime = TimeSpan.FromMinutes(30);

        private readonly IDataProtector _protector;

        public AccountTokenService(IDataProtectionProvider dataProtectionProvider)
        {
            _protector = dataProtectionProvider.CreateProtector(Purpose);
        }

        public string GenerateToken(User user)
        {
            string payload = $"{user.Id}|{user.Email}|{DateTime.UtcNow.Ticks}";
            byte[] protectedBytes = _protector.Protect(Encoding.UTF8.GetBytes(payload));
            return WebEncoders.Base64UrlEncode(protectedBytes);
        }

        public bool TryValidateToken(string token, out int userId, out string email)
        {
            userId = 0;
            email = string.Empty;

            if (string.IsNullOrWhiteSpace(token))
                return false;

            try
            {
                byte[] protectedBytes = WebEncoders.Base64UrlDecode(token);
                byte[] plainBytes = _protector.Unprotect(protectedBytes);
                string payload = Encoding.UTF8.GetString(plainBytes);

                var parts = payload.Split('|');
                if (parts.Length != 3)
                    return false;

                if (!int.TryParse(parts[0], out userId))
                    return false;

                email = parts[1];

                if (!long.TryParse(parts[2], out long ticks))
                    return false;

                var createdAtUtc = new DateTime(ticks, DateTimeKind.Utc);
                if (DateTime.UtcNow - createdAtUtc > TokenLifetime)
                    return false;

                return true;
            }
            catch
            {

                return false;
            }
        }
    }
}

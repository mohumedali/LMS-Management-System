using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Models
{
    public class Setting
    {
        public int Id { get; set; }

        // General
        public string SystemName { get; set; } = "Course Management System";

        public string SupportEmail { get; set; } = string.Empty;

        public string SupportPhone { get; set; } = string.Empty;

        public string DefaultLanguage { get; set; } = "English";

        public bool AllowRegistration { get; set; } = true;

        public bool AutoApproveEnrollment { get; set; }

        public bool EnableCertificates { get; set; }

        // Admin Profile
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string? ProfileImage { get; set; }
    }
}

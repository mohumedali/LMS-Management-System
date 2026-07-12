using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CourseManagment.DAL.Enums;

namespace CourseManagment.DAL.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public PaymentStatus Status { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public string? TransactionId { get; set; }

        // Enrollment

        public int? EnrollmentId { get; set; }

        public Enrollment? Enrollment { get; set; }
    }
}
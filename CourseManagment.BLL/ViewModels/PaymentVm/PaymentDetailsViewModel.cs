using CourseManagment.DAL.Enums;

namespace CourseManagment.BLL.ViewModels.PaymentVm
{
    public class PaymentDetailsViewModel
    {
        public int Id { get; set; }

        // Added so the "Try Again" button on the Failed page
        // can send the user back to Create with the right course.
        public int? EnrollmentId { get; set; }

        public string UserName { get; set; }

        public string CourseTitle { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public PaymentStatus Status { get; set; }

        public DateTime PaymentDate { get; set; }

        public string? TransactionId { get; set; }
    }
}

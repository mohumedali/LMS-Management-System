using CourseManagment.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace CourseManagment.BLL.ViewModels.PaymentVm
{
    public class CreatePaymentViewModel
    {
        public int EnrollmentId { get; set; }

        public decimal Amount { get; set; }

        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; }

        // ==========================================
        // Order summary (display only - populated by
        // the controller, round-tripped via hidden
        // inputs so it survives validation postbacks)
        // ==========================================
        public string? CourseTitle { get; set; }

        public string? CourseImage { get; set; }

        // ==========================================
        // Demo card fields
        // NOTE: these are NEVER persisted to the database.
        // They only exist to make the demo checkout feel
        // like a real payment form and to drive the
        // simulated success/decline logic in the controller.
        // ==========================================
        [Display(Name = "Cardholder Name")]
        [StringLength(100)]
        public string? CardHolderName { get; set; }

        [Display(Name = "Card Number")]
        [StringLength(19, MinimumLength = 12, ErrorMessage = "Enter a valid card number.")]
        public string? CardNumber { get; set; }

        [Display(Name = "Expiry Date")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Use MM/YY format.")]
        public string? ExpiryDate { get; set; }

        [Display(Name = "CVV")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "Enter a valid CVV.")]
        public string? CVV { get; set; }
    }
}

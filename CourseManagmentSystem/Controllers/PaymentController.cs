using CourseManagment.BLL.Services;
using CourseManagment.BLL.ViewModels.PaymentVm;
using CourseManagment.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseManagment.PL.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IEnrollmentService _enrollmentService;

        public PaymentController(
            IPaymentService paymentService,
            IEnrollmentService enrollmentService)
        {
            _paymentService = paymentService;
            _enrollmentService = enrollmentService;
        }

        private int GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("User is not logged in.");

            return int.Parse(userId);
        }

        #region My Payments

        [HttpGet]
        public async Task<IActionResult> Index(
            CancellationToken ct)
        {
            var userId = GetUserId();

            var payments = await _paymentService
                .GetUserPaymentsAsync(userId, ct);

            return View(payments);
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(
            int id,
            CancellationToken ct)
        {
            var payment = await _paymentService
                .GetPaymentAsync(id, ct);

            if (payment == null)
                return NotFound();

            return View(payment);
        }

        #endregion

        #region Create (Demo Checkout)

        [HttpGet]
        public async Task<IActionResult> Create(
            int enrollmentId,
            decimal amount,
            CancellationToken ct)
        {
            // Pull the course info just so the checkout page can
            // show a proper order summary (image / title / price).
            var enrollment = await _enrollmentService
                .GetEnrollmentDetailsAsync(enrollmentId, ct);

            var model = new CreatePaymentViewModel
            {
                EnrollmentId = enrollmentId,
                Amount = amount,
                CourseTitle = enrollment?.Title,
                CourseImage = enrollment?.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreatePaymentViewModel model,
            CancellationToken ct)
        {
            bool isCardPayment =
                model.PaymentMethod == PaymentMethod.Visa ||
                model.PaymentMethod == PaymentMethod.MasterCard;

            // Card fields are only required when the user chose
            // Visa / MasterCard as the payment method.
            if (isCardPayment)
            {
                ValidateCardFields(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var paymentId = await _paymentService.CreatePaymentAsync(
                    model,
                    ct);

                // Simulate a real gateway round-trip so the demo
                // actually feels like it's "processing" a payment.
                await Task.Delay(1200, ct);

                var cardDigits = (model.CardNumber ?? string.Empty)
                    .Replace(" ", string.Empty);

                // Demo-only rule: a card number ending in 0000
                // simulates a declined payment, anything else succeeds.
                var isDeclined = isCardPayment && cardDigits.EndsWith("0000");

                if (isDeclined)
                {
                    await _paymentService.FailPaymentAsync(paymentId, ct);

                    return RedirectToAction(
                        nameof(Failed),
                        new { id = paymentId });
                }

                await _paymentService.CompletePaymentAsync(paymentId, ct);

                return RedirectToAction(
                    nameof(Success),
                    new { id = paymentId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(model);
            }
        }

        private void ValidateCardFields(CreatePaymentViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.CardHolderName))
            {
                ModelState.AddModelError(
                    nameof(model.CardHolderName),
                    "Cardholder name is required.");
            }

            var digitsOnly = (model.CardNumber ?? string.Empty)
                .Replace(" ", string.Empty);

            if (digitsOnly.Length < 12 || !digitsOnly.All(char.IsDigit))
            {
                ModelState.AddModelError(
                    nameof(model.CardNumber),
                    "Enter a valid card number.");
            }

            if (string.IsNullOrWhiteSpace(model.ExpiryDate) ||
                !System.Text.RegularExpressions.Regex.IsMatch(
                    model.ExpiryDate,
                    @"^(0[1-9]|1[0-2])\/\d{2}$"))
            {
                ModelState.AddModelError(
                    nameof(model.ExpiryDate),
                    "Use MM/YY format.");
            }

            if (string.IsNullOrWhiteSpace(model.CVV) ||
                model.CVV.Length < 3 ||
                !model.CVV.All(char.IsDigit))
            {
                ModelState.AddModelError(
                    nameof(model.CVV),
                    "Enter a valid CVV.");
            }
        }

        #endregion

        #region Success (Demo)

        [HttpGet]
        public async Task<IActionResult> Success(
            int id,
            CancellationToken ct)
        {
            var payment =
                await _paymentService.GetPaymentAsync(
                    id,
                    ct);

            if (payment == null)
                return NotFound();

            return View(payment);
        }

        #endregion

        #region Failed (Demo)

        [HttpGet]
        public async Task<IActionResult> Failed(
            int id,
            CancellationToken ct)
        {
            var payment =
                await _paymentService.GetPaymentAsync(
                    id,
                    ct);

            if (payment == null)
                return NotFound();

            return View(payment);
        }

        #endregion

        #region Receipt

        [HttpGet]
        public async Task<IActionResult> Receipt(
            int id,
            CancellationToken ct)
        {
            var payment =
                await _paymentService.GetPaymentAsync(
                    id,
                    ct);

            if (payment == null)
                return NotFound();

            return View(payment);
        }

        #endregion

        #region Cancel

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(
            int id,
            CancellationToken ct)
        {
            var result =
                await _paymentService
                    .CancelPaymentAsync(id, ct);

            if (!result)
                return NotFound();

            TempData["Success"] =
                "Payment cancelled successfully.";

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}

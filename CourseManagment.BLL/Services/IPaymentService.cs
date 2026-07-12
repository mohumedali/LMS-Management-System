
using CourseManagment.BLL.ViewModels.PaymentVm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services
{
    public interface IPaymentService
    {
        Task<int> CreatePaymentAsync(
            CreatePaymentViewModel model,
            CancellationToken ct);

        Task<PaymentDetailsViewModel?> GetPaymentAsync(
            int paymentId,
            CancellationToken ct);

        Task<IEnumerable<PaymentCardViewModel>> GetUserPaymentsAsync(
            int userId,
            CancellationToken ct);

        Task<bool> CompletePaymentAsync(
            int paymentId,
            CancellationToken ct);

        Task<bool> FailPaymentAsync(
            int paymentId,
            CancellationToken ct);

        Task<bool> CancelPaymentAsync(
            int paymentId,
            CancellationToken ct);

        Task<bool> PaymentExistsForEnrollmentAsync(
            int enrollmentId,
            CancellationToken ct);
    }
}
using CourseManagment.BLL.Services;
using CourseManagment.BLL.ViewModels.PaymentVm;
using CourseManagment.DAL.Enums;
using CourseManagment.DAL.Interfaces;
using CourseManagment.DAL.Models;
using CourseManagment.DAL.Repositories;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepo;
    private readonly IEnrollmentRepo _enrollmentRepo;

    public PaymentService(
        IPaymentRepository paymentRepo,
        IEnrollmentRepo enrollmentRepo)
    {
        _paymentRepo = paymentRepo;
        _enrollmentRepo = enrollmentRepo;
    }

    public async Task<int> CreatePaymentAsync(
        CreatePaymentViewModel model,
        CancellationToken ct)
    {
        var enrollment = await _enrollmentRepo.GetByIdAsync(
            model.EnrollmentId,
            ct);

        if (enrollment == null)
            throw new Exception("Enrollment not found.");

        var exists = await _paymentRepo.GetByEnrollmentIdAsync(
            model.EnrollmentId,
            ct);


        if (exists != null && exists.Status == PaymentStatus.Paid)
            throw new Exception("Payment already exists.");

        if (exists != null)
        {
            exists.Amount = model.Amount;
            exists.PaymentMethod = model.PaymentMethod;
            exists.Status = PaymentStatus.Pending;
            exists.PaymentDate = DateTime.UtcNow;
            exists.TransactionId = Guid.NewGuid()
                .ToString("N")
                .Substring(0, 12)
                .ToUpper();

            await _paymentRepo.UpdateAsync(exists, ct);

            Console.WriteLine($"Payment Id = {exists.Id} (reused)");

            return exists.Id;
        }

        var payment = new Payment
        {
            EnrollmentId = enrollment.Id,


            Amount = model.Amount,
            PaymentMethod = model.PaymentMethod,
            Status = PaymentStatus.Pending,
            PaymentDate = DateTime.UtcNow,
            TransactionId = Guid.NewGuid()
                .ToString("N")
                .Substring(0, 12)
                .ToUpper()
        };

        await _paymentRepo.AddAsync(payment, ct);

        Console.WriteLine($"Payment Id = {payment.Id}");

        return payment.Id;
    }

    public async Task<PaymentDetailsViewModel?> GetPaymentAsync(
        int paymentId,
        CancellationToken ct)
    {
        var payment = await _paymentRepo.GetByIdAsync(paymentId, ct);

        if (payment == null)
            return null;

        return new PaymentDetailsViewModel
        {
            Id = payment.Id,
            EnrollmentId = payment.EnrollmentId,
            Amount = payment.Amount,
            PaymentMethod = payment.PaymentMethod,
            Status = payment.Status,
            PaymentDate = payment.PaymentDate,
            TransactionId = payment.TransactionId,

            CourseTitle = payment.Enrollment?.Course?.Title ?? "",
            UserName = payment.Enrollment?.User?.FullName ?? ""
        };
    }

    public async Task<IEnumerable<PaymentCardViewModel>> GetUserPaymentsAsync(
        int userId,
        CancellationToken ct)
    {
        var payments = await _paymentRepo.GetUserPaymentsAsync(userId, ct);

        return payments.Select(p => new PaymentCardViewModel
        {
            Id = p.Id,
            CourseTitle = p.Enrollment?.Course?.Title ?? "",
            Amount = p.Amount,
            Status = p.Status,
            PaymentDate = p.PaymentDate
        });
    }

    public async Task<bool> CompletePaymentAsync(
        int paymentId,
        CancellationToken ct)
    {
        var payment = await _paymentRepo.GetByIdAsync(paymentId, ct);

        if (payment == null)
            return false;

        payment.Status = PaymentStatus.Paid;

        if (payment.Enrollment != null)
        {
            payment.Enrollment.Status = EnrollmentStatus.Active;
        }

        await _paymentRepo.UpdateAsync(payment, ct);

        return true;
    }
    public async Task<bool> FailPaymentAsync(
        int paymentId,
        CancellationToken ct)
    {
        var payment = await _paymentRepo.GetByIdAsync(paymentId, ct);

        if (payment == null)
            return false;

        payment.Status = PaymentStatus.Failed;
        payment.Enrollment!.Status = EnrollmentStatus.Cancelled;


        await _paymentRepo.UpdateAsync(payment, ct);

        return true;
    }

    public async Task<bool> CancelPaymentAsync(
        int paymentId,
        CancellationToken ct)
    {
        var payment = await _paymentRepo.GetByIdAsync(paymentId, ct);

        if (payment == null)
            return false;

        payment.Status = PaymentStatus.Cancelled;

        await _paymentRepo.UpdateAsync(payment, ct);

        return true;
    }

    public async Task<bool> PaymentExistsForEnrollmentAsync(
        int enrollmentId,
        CancellationToken ct)
    {
        return await _paymentRepo.GetByEnrollmentIdAsync(
            enrollmentId,
            ct) != null;
    }
}
namespace KioskPos.Analytics.PosIntegration.Models;

public class PosPayment
{
    public int PaymentMethodId { get; set; }
    public string PaymentMethodName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal ChangeAmount { get; set; }
    public decimal TipAmount { get; set; }
    public DateTime Date { get; set; }
    public bool IsPrepayment { get; set; }
    public string? ExtraInformation { get; set; }
}

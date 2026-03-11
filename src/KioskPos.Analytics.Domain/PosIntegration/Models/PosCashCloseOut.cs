namespace KioskPos.Analytics.PosIntegration.Models;

public class PosCashCloseOut
{
    public string? BusinessDay { get; set; }
    public DateTime Date { get; set; }
    public int? PosId { get; set; }
    public string? PosName { get; set; }
    public int? UserId { get; set; }
    public string? UserName { get; set; }
    public PosTotals? Totals { get; set; }
    public List<PosCashCloseOutPaymentMethod> PaymentMethods { get; set; } = new();
}

public class PosCashCloseOutPaymentMethod
{
    public int PaymentMethodId { get; set; }
    public string PaymentMethodName { get; set; } = string.Empty;
    public decimal ExpectedAmount { get; set; }
    public decimal RealAmount { get; set; }
    public decimal Difference { get; set; }
}

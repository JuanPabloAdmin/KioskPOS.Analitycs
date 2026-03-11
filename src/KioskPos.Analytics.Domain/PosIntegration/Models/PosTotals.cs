namespace KioskPos.Analytics.PosIntegration.Models;

public class PosTotals
{
    public decimal GrossAmount { get; set; }
    public decimal NetAmount { get; set; }
    public decimal VatAmount { get; set; }
    public decimal SurchargeAmount { get; set; }
    public List<PosTaxDetail> Taxes { get; set; } = new();
}

public class PosTaxDetail
{
    public decimal VatRate { get; set; }
    public decimal SurchargeRate { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal NetAmount { get; set; }
    public decimal VatAmount { get; set; }
    public decimal SurchargeAmount { get; set; }
}

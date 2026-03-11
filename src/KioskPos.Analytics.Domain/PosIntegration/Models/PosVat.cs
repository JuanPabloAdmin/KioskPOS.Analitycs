namespace KioskPos.Analytics.PosIntegration.Models;

public class PosVat
{
    public int ExternalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal VatRate { get; set; }
    public decimal SurchargeRate { get; set; }
    public bool Enabled { get; set; } = true;
}

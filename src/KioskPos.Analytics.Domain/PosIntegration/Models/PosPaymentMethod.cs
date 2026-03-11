namespace KioskPos.Analytics.PosIntegration.Models;

public class PosPaymentMethod
{
    public int ExternalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool GiveChange { get; set; }
    public bool IncludeInBalance { get; set; }
    public bool IsValidForSale { get; set; }
    public bool IsDeleted { get; set; }
}

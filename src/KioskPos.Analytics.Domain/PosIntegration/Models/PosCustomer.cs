namespace KioskPos.Analytics.PosIntegration.Models;

public class PosCustomer
{
    public int ExternalId { get; set; }
    public string FiscalName { get; set; } = string.Empty;
    public string? BusinessName { get; set; }
    public string? Cif { get; set; }
    public string? CardNumber { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public bool IsDeleted { get; set; }
}

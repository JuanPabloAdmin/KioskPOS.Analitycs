namespace KioskPos.Analytics.PosIntegration.Models;

public class PosSaleCenter
{
    public int ExternalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int PriceListId { get; set; }
    public bool VatIncluded { get; set; }
    public List<PosSaleLocation> Locations { get; set; } = new();
    public bool IsDeleted { get; set; }
}

public class PosSaleLocation
{
    public string Name { get; set; } = string.Empty;
}

namespace KioskPos.Analytics.PosIntegration.Models;

public class PosFamily
{
    public int ExternalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentFamilyId { get; set; }
    public string? Color { get; set; }
    public int? Order { get; set; }
    public bool IsDeleted { get; set; }
}

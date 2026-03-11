namespace KioskPos.Analytics.PosIntegration.Models;

public class PosUser
{
    public int ExternalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Profile { get; set; }
    public bool IsDeleted { get; set; }
}

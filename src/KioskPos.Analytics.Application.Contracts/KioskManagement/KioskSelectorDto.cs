using System;

namespace KioskPos.Analytics.KioskManagement;

/// <summary>
/// DTO ligero para el selector de kioscos en el frontend.
/// </summary>
public class KioskSelectorDto
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Color { get; set; } = "#6366f1";
    public string Icon { get; set; } = "🖥️";
    public int WorkplaceId { get; set; }
}

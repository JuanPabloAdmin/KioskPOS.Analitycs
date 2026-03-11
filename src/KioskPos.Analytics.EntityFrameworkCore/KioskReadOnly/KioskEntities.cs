namespace KioskPos.Analytics.EntityFrameworkCore.KioskReadOnly;

/// <summary>
/// Mapeo read-only de la tabla Configurations del Kiosk (key-value).
/// </summary>
public class KioskConfigurationEntity
{
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
}

/// <summary>
/// Mapeo read-only de la tabla SelectedFamilies del Kiosk.
/// </summary>
public class KioskSelectedFamilyEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}

/// <summary>
/// Mapeo read-only de la tabla Sessions del Kiosk.
/// </summary>
public class KioskSessionEntity
{
    public int Id { get; set; }
    public DateTime? CreatedAt { get; set; }
}

/// <summary>
/// Mapeo read-only de la tabla Logs del Kiosk.
/// </summary>
public class KioskLogEntity
{
    public int Id { get; set; }
    public string? Method { get; set; }
    public string? Url { get; set; }
    public string? Message { get; set; }
    public DateTime? CreatedAt { get; set; }
}

/// <summary>
/// Mapeo read-only de la tabla CheckoutOperations del Kiosk.
/// </summary>
public class KioskCheckoutOperationEntity
{
    public Guid Id { get; set; }
    public int? SessionId { get; set; }
    public bool? PaymentApproved { get; set; }
    public string? PaytefOperationNumber { get; set; }
    public string? AgoraGlobalId { get; set; }
    public int? ImportAttempts { get; set; }
    public string? LastError { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
}

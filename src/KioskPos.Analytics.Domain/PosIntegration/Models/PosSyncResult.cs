namespace KioskPos.Analytics.PosIntegration.Models;

public class PosSyncResult
{
    public bool Success { get; set; }
    public SyncStatus Status { get; set; }
    public int RecordsSynced { get; set; }
    public DateTime SyncDate { get; set; } = DateTime.UtcNow;
    public string? ErrorMessage { get; set; }
    public TimeSpan Duration { get; set; }
}

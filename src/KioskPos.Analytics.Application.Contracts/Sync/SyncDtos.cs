using System;

namespace KioskPos.Analytics.Sync;

public class SyncResultDto
{
    public bool Queued { get; set; }
    public string BusinessDay { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class SyncHistoryDto
{
    public Guid Id { get; set; }
    public string? SyncType { get; set; }
    public string? BusinessDay { get; set; }
    public string Status { get; set; } = string.Empty;
    public int RecordsSynced { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Details { get; set; }
    public double? DurationSeconds { get; set; }
}

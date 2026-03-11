using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace KioskPos.Analytics.Analytics.Entities;

/// <summary>
/// Registro histórico de cada ejecución de sincronización.
/// </summary>
public class SyncHistory : CreationAuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid PosConnectionConfigId { get; set; }
    public PosIntegration.SyncStatus Status { get; set; }
    public string? SyncType { get; set; }
    public DateOnly? BusinessDay { get; set; }
    public int RecordsSynced { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Details { get; set; }
}

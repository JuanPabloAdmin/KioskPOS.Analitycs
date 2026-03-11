using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace KioskPos.Analytics.Analytics.Entities;

/// <summary>
/// Factura cacheada desde el POS para consultas analíticas rápidas.
/// </summary>
public class CachedInvoice : CreationAuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid PosConnectionConfigId { get; set; }

    public string Serie { get; set; } = string.Empty;
    public int Number { get; set; }
    public string? GlobalId { get; set; }
    public DateOnly BusinessDay { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string? DocumentType { get; set; }
    public bool VatIncluded { get; set; }

    public decimal GrossAmount { get; set; }
    public decimal NetAmount { get; set; }
    public decimal VatAmount { get; set; }

    public int? ExternalPosId { get; set; }
    public int? ExternalUserId { get; set; }
    public string? UserName { get; set; }
    public int? ExternalCustomerId { get; set; }
    public string? CustomerName { get; set; }

    public string? PrimaryPaymentMethod { get; set; }
    public string? LinesJson { get; set; }
    public string? PaymentsJson { get; set; }

    public DateTime? PosProcessedDate { get; set; }
    public int LineCount { get; set; }
}

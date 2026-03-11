using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace KioskPos.Analytics.Analytics.Entities;

/// <summary>
/// Pedido de venta cacheado desde el POS para consultas analíticas rápidas.
/// </summary>
public class CachedSalesOrder : CreationAuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid PosConnectionConfigId { get; set; }

    // Datos del pedido
    public string Serie { get; set; } = string.Empty;
    public int Number { get; set; }
    public string? GlobalId { get; set; }
    public DateOnly BusinessDay { get; set; }
    public DateTime OrderDate { get; set; }
    public string? Status { get; set; }
    public PosIntegration.PosOrderType OrderType { get; set; }

    // Totales
    public decimal GrossAmount { get; set; }
    public decimal NetAmount { get; set; }
    public decimal VatAmount { get; set; }

    // Referencias
    public int? ExternalPosId { get; set; }
    public int? ExternalUserId { get; set; }
    public string? UserName { get; set; }
    public int? ExternalCustomerId { get; set; }
    public int? ExternalSaleCenterId { get; set; }
    public string? SaleCenterName { get; set; }
    public string? SaleLocationName { get; set; }
    public int? Guests { get; set; }

    // Pago
    public string? PrimaryPaymentMethod { get; set; }

    // Líneas serializadas (JSON) para detalle bajo demanda
    public string? LinesJson { get; set; }
    public string? PaymentsJson { get; set; }

    // Control
    public DateTime? PosProcessedDate { get; set; }
    public int LineCount { get; set; }
    public int ItemCount { get; set; }
}

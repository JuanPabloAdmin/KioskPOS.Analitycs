using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace KioskPos.Analytics.Analytics.Entities;

/// <summary>
/// Snapshot precalculado de KPIs diarios para carga rápida del dashboard.
/// </summary>
public class DashboardSnapshot : CreationAuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid PosConnectionConfigId { get; set; }
    public DateOnly BusinessDay { get; set; }

    // KPIs principales
    public decimal TotalRevenue { get; set; }
    public decimal TotalNetRevenue { get; set; }
    public int TotalOrders { get; set; }
    public int TotalInvoices { get; set; }
    public decimal AverageTicket { get; set; }
    public int TotalItemsSold { get; set; }
    public int TotalGuests { get; set; }

    // Desglose por tipo de pedido
    public int TableOrders { get; set; }
    public int TakeAwayOrders { get; set; }
    public int DeliveryOrders { get; set; }

    // Desglose por forma de pago (JSON)
    public string? PaymentBreakdownJson { get; set; }

    // Top productos (JSON)
    public string? TopProductsJson { get; set; }

    // Top familias (JSON)
    public string? TopFamiliesJson { get; set; }

    // Ventas por hora (JSON)
    public string? HourlySalesJson { get; set; }
}

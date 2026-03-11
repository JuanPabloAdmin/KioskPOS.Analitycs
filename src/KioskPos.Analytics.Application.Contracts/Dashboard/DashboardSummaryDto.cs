using System;
using System.Collections.Generic;

namespace KioskPos.Analytics.Dashboard;

/// <summary>
/// DTO principal del Dashboard — VERSIÓN MEJORADA con KPIs extendidos.
/// REEMPLAZA el DashboardSummaryDto anterior.
/// </summary>
public class DashboardSummaryDto
{
    public string BusinessDay { get; set; } = string.Empty;

    // ── KPIs principales ──
    public decimal TotalRevenue { get; set; }
    public decimal TotalNetRevenue { get; set; }
    public int TotalOrders { get; set; }
    public int TotalInvoices { get; set; }
    public decimal AverageTicket { get; set; }
    public int TotalItemsSold { get; set; }
    public int TotalGuests { get; set; }

    // ── KPIs NUEVOS ──
    public decimal VatCollected { get; set; }
    public string PeakHour { get; set; } = "—";
    public decimal PeakHourRevenue { get; set; }
    public int PeakHourOrders { get; set; }
    public int LiveOrdersCount { get; set; }
    public int LiveTicketsCount { get; set; }

    // ── Comparativa con ayer (%) ──
    public decimal? RevenueChangePercent { get; set; }
    public decimal? OrdersChangePercent { get; set; }
    public decimal? TicketChangePercent { get; set; }
    public decimal? ItemsChangePercent { get; set; }
    public decimal? YesterdayRevenue { get; set; }
    public int? YesterdayOrders { get; set; }

    // ── Desglose por tipo de pedido ──
    public int TableOrders { get; set; }
    public int TakeAwayOrders { get; set; }
    public int DeliveryOrders { get; set; }
    public decimal TableRevenue { get; set; }
    public decimal TakeAwayRevenue { get; set; }
    public decimal DeliveryRevenue { get; set; }

    // ── Desglose por forma de pago ──
    public List<PaymentBreakdownDto> PaymentBreakdown { get; set; } = new();

    // ── Top productos ──
    public List<TopProductDto> TopProducts { get; set; } = new();

    // ── Top familias ──
    public List<TopFamilyDto> TopFamilies { get; set; } = new();

    // ── Ventas por hora ──
    public List<HourlySalesDto> HourlySales { get; set; } = new();

    // ── Multi-kiosk ──
    public string? KioskId { get; set; }
    public string? KioskName { get; set; }
    public string? KioskColor { get; set; }
    public string? KioskIcon { get; set; }
}

public class PaymentBreakdownDto
{
    public string PaymentMethodName { get; set; } = string.Empty;
    public int TransactionCount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Percentage { get; set; }
}

public class TopProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? FamilyName { get; set; }
    public int QuantitySold { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AveragePrice { get; set; }
    public int OrderAppearances { get; set; }
}

public class TopFamilyDto
{
    public int FamilyId { get; set; }
    public string FamilyName { get; set; } = string.Empty;
    public int ItemsSold { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal Percentage { get; set; }
}

public class HourlySalesDto
{
    public int Hour { get; set; }
    public string HourLabel { get; set; } = string.Empty;
    public int OrderCount { get; set; }
    public decimal Revenue { get; set; }
    public decimal AverageTicket { get; set; }
    public int ItemCount { get; set; }
}

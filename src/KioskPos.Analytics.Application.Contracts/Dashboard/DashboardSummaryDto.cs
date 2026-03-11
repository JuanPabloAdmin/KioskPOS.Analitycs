using System;
using System.Collections.Generic;

namespace KioskPos.Analytics.Dashboard;

/// <summary>
/// DTO principal del Dashboard — resumen de un día de negocio.
/// </summary>
public class DashboardSummaryDto
{
    public string BusinessDay { get; set; } = string.Empty;
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

    // Desglose por forma de pago
    public List<PaymentBreakdownDto> PaymentBreakdown { get; set; } = new();

    // Top productos
    public List<TopProductDto> TopProducts { get; set; } = new();

    // Top familias
    public List<TopFamilyDto> TopFamilies { get; set; } = new();

    // Ventas por hora
    public List<HourlySalesDto> HourlySales { get; set; } = new();
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
}

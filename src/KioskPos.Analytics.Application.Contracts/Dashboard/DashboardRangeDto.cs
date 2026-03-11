using System;
using System.Collections.Generic;

namespace KioskPos.Analytics.Dashboard;

/// <summary>
/// DTO para analíticas de un rango de fechas.
/// </summary>
public class DashboardRangeDto
{
    public string FromDate { get; set; } = string.Empty;
    public string ToDate { get; set; } = string.Empty;
    public int TotalDays { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
    public decimal AverageTicket { get; set; }
    public decimal AverageDailyRevenue { get; set; }
    public int AverageDailyOrders { get; set; }
    public List<DailySummaryDto> DailySummaries { get; set; } = new();
}

public class DailySummaryDto
{
    public string BusinessDay { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public int OrderCount { get; set; }
    public decimal AverageTicket { get; set; }
}

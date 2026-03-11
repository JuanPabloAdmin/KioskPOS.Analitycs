using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KioskPos.Analytics.PosIntegration;
using KioskPos.Analytics.PosIntegration.Interfaces;
using KioskPos.Analytics.PosIntegration.Models;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using KioskPos.Analytics.Permissions;
namespace KioskPos.Analytics.Dashboard;

[Authorize(AnalyticsPermissions.Dashboard.Default)]
public class DashboardAppService : ApplicationService, IDashboardAppService
{
    private readonly IPosIntegrationService _posService;

    public DashboardAppService(IPosIntegrationService posService)
    {
        _posService = posService;
    }

    public async Task<DashboardSummaryDto> GetTodaySummaryAsync()
    {
        return await GetDailySummaryAsync(DateTime.Today);
    }

    public async Task<DashboardSummaryDto> GetDailySummaryAsync(DateTime businessDay)
    {
        var day = DateOnly.FromDateTime(businessDay);

        // Obtener datos del POS en paralelo
        var ordersTask = _posService.GetSalesOrdersAsync(day, includeProcessed: true);
        var invoicesTask = _posService.GetInvoicesAsync(day, includeProcessed: true);

        await Task.WhenAll(ordersTask, invoicesTask);

        var orders = ordersTask.Result;
        var invoices = invoicesTask.Result;

        // Construir el resumen
        var summary = new DashboardSummaryDto
        {
            BusinessDay = day.ToString("yyyy-MM-dd"),
            TotalOrders = orders.Count,
            TotalInvoices = invoices.Count,
            TotalRevenue = orders.Sum(o => o.Totals.GrossAmount),
            TotalNetRevenue = orders.Sum(o => o.Totals.NetAmount),
            TotalItemsSold = orders.SelectMany(o => o.Lines)
                .Where(l => l.LineType != "MenuHeader")
                .Sum(l => (int)Math.Abs(l.Quantity)),
            TotalGuests = orders.Sum(o => o.Guests ?? 1),
            AverageTicket = orders.Count > 0
                ? orders.Sum(o => o.Totals.GrossAmount) / orders.Count
                : 0,

            // Desglose por tipo
            TableOrders = orders.Count(o => o.OrderType == PosOrderType.Table),
            TakeAwayOrders = orders.Count(o => o.OrderType == PosOrderType.TakeAway),
            DeliveryOrders = orders.Count(o => o.OrderType == PosOrderType.Delivery),

            // Desglose por forma de pago
            PaymentBreakdown = BuildPaymentBreakdown(orders),

            // Top 10 productos
            TopProducts = BuildTopProducts(orders, 10),

            // Top familias
            TopFamilies = BuildTopFamilies(orders),

            // Ventas por hora
            HourlySales = BuildHourlySales(orders)
        };

        return summary;
    }

    public async Task<DashboardRangeDto> GetRangeSummaryAsync(DateTime fromDate, DateTime toDate)
    {
        var result = new DashboardRangeDto
        {
            FromDate = fromDate.ToString("yyyy-MM-dd"),
            ToDate = toDate.ToString("yyyy-MM-dd"),
            DailySummaries = new List<DailySummaryDto>()
        };

        // Iterar cada día del rango
        for (var date = fromDate.Date; date <= toDate.Date; date = date.AddDays(1))
        {
            var day = DateOnly.FromDateTime(date);
            var orders = await _posService.GetSalesOrdersAsync(day, includeProcessed: true);

            var dailyRevenue = orders.Sum(o => o.Totals.GrossAmount);
            var dailyOrders = orders.Count;

            result.DailySummaries.Add(new DailySummaryDto
            {
                BusinessDay = day.ToString("yyyy-MM-dd"),
                Revenue = dailyRevenue,
                OrderCount = dailyOrders,
                AverageTicket = dailyOrders > 0 ? dailyRevenue / dailyOrders : 0
            });
        }

        result.TotalDays = result.DailySummaries.Count;
        result.TotalRevenue = result.DailySummaries.Sum(d => d.Revenue);
        result.TotalOrders = result.DailySummaries.Sum(d => d.OrderCount);
        result.AverageTicket = result.TotalOrders > 0 ? result.TotalRevenue / result.TotalOrders : 0;
        result.AverageDailyRevenue = result.TotalDays > 0 ? result.TotalRevenue / result.TotalDays : 0;
        result.AverageDailyOrders = result.TotalDays > 0 ? result.TotalOrders / result.TotalDays : 0;

        return result;
    }

    public async Task<bool> TestPosConnectionAsync()
    {
        return await _posService.TestConnectionAsync();
    }

    // ── Métodos privados de cálculo ──

    private static List<PaymentBreakdownDto> BuildPaymentBreakdown(List<PosSalesOrder> orders)
    {
        var allPayments = orders.SelectMany(o => o.Payments).ToList();
        var totalAmount = allPayments.Sum(p => p.Amount);

        return allPayments
            .GroupBy(p => p.PaymentMethodName)
            .Select(g => new PaymentBreakdownDto
            {
                PaymentMethodName = g.Key,
                TransactionCount = g.Count(),
                TotalAmount = g.Sum(p => p.Amount),
                Percentage = totalAmount > 0 ? Math.Round(g.Sum(p => p.Amount) / totalAmount * 100, 2) : 0
            })
            .OrderByDescending(p => p.TotalAmount)
            .ToList();
    }

    private static List<TopProductDto> BuildTopProducts(List<PosSalesOrder> orders, int count)
    {
        return orders
            .SelectMany(o => o.Lines)
            .Where(l => l.LineType != "MenuHeader" && l.Quantity > 0)
            .GroupBy(l => new { l.ProductId, l.ProductName, l.FamilyName })
            .Select(g => new TopProductDto
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.ProductName,
                FamilyName = g.Key.FamilyName,
                QuantitySold = (int)g.Sum(l => l.Quantity),
                TotalRevenue = g.Sum(l => l.TotalAmount)
            })
            .OrderByDescending(p => p.QuantitySold)
            .Take(count)
            .ToList();
    }

    private static List<TopFamilyDto> BuildTopFamilies(List<PosSalesOrder> orders)
    {
        var lines = orders.SelectMany(o => o.Lines)
            .Where(l => l.FamilyId.HasValue && l.LineType != "MenuHeader" && l.Quantity > 0)
            .ToList();

        var totalRevenue = lines.Sum(l => l.TotalAmount);

        return lines
            .GroupBy(l => new { l.FamilyId, l.FamilyName })
            .Select(g => new TopFamilyDto
            {
                FamilyId = g.Key.FamilyId ?? 0,
                FamilyName = g.Key.FamilyName ?? "Sin familia",
                ItemsSold = (int)g.Sum(l => l.Quantity),
                TotalRevenue = g.Sum(l => l.TotalAmount),
                Percentage = totalRevenue > 0 ? Math.Round(g.Sum(l => l.TotalAmount) / totalRevenue * 100, 2) : 0
            })
            .OrderByDescending(f => f.TotalRevenue)
            .ToList();
    }

    private static List<HourlySalesDto> BuildHourlySales(List<PosSalesOrder> orders)
    {
        // Generar todas las horas del día (0-23)
        var hourly = Enumerable.Range(0, 24)
            .Select(h =>
            {
                var hourOrders = orders.Where(o => o.Date.Hour == h).ToList();
                return new HourlySalesDto
                {
                    Hour = h,
                    HourLabel = $"{h:D2}:00",
                    OrderCount = hourOrders.Count,
                    Revenue = hourOrders.Sum(o => o.Totals.GrossAmount)
                };
            })
            .ToList();

        return hourly;
    }
}

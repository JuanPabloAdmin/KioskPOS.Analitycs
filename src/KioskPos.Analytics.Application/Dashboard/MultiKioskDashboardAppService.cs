using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KioskPos.Analytics.Analytics.Entities;
using KioskPos.Analytics.KioskManagement;
using KioskPos.Analytics.PosIntegration;
using KioskPos.Analytics.PosIntegration.Interfaces;
using KioskPos.Analytics.PosIntegration.Models;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace KioskPos.Analytics.Dashboard;


public class MultiKioskDashboardAppService : ApplicationService, IDashboardAppService
{
    private readonly IPosIntegrationService _posService;
    private readonly IRepository<KioskRegistration, Guid> _kioskRepo;

    public MultiKioskDashboardAppService(
        IPosIntegrationService posService,
        IRepository<KioskRegistration, Guid> kioskRepo)
    {
        _posService = posService;
        _kioskRepo = kioskRepo;
    }

    public async Task<DashboardSummaryDto> GetTodaySummaryAsync()
    {
        return await GetDailySummaryAsync(DateTime.Today);
    }


    public async Task<DashboardSummaryDto> GetDailySummaryAsync(DateTime businessDay)
    {
        var day = DateOnly.FromDateTime(businessDay);
        var yesterday = day.AddDays(-1);

        // Obtener datos de hoy y ayer en paralelo
        var ordersTask = _posService.GetSalesOrdersAsync(day, includeProcessed: true);
        var invoicesTask = _posService.GetInvoicesAsync(day, includeProcessed: true);
        var yesterdayOrdersTask = _posService.GetSalesOrdersAsync(yesterday, includeProcessed: true);

        await Task.WhenAll(ordersTask, invoicesTask, yesterdayOrdersTask);

        return BuildSummary(day, ordersTask.Result, invoicesTask.Result, yesterdayOrdersTask.Result);
    }

    public async Task<DashboardSummaryDto> GetKioskDailySummaryAsync(DateTime businessDay, Guid kioskId)
    {
        var day = DateOnly.FromDateTime(businessDay);
        var yesterday = day.AddDays(-1);
        var kiosk = await _kioskRepo.GetAsync(kioskId);

        var allOrdersTask = _posService.GetSalesOrdersAsync(day, includeProcessed: true);
        var allInvoicesTask = _posService.GetInvoicesAsync(day, includeProcessed: true);
        var yesterdayAllTask = _posService.GetSalesOrdersAsync(yesterday, includeProcessed: true);

        await Task.WhenAll(allOrdersTask, allInvoicesTask, yesterdayAllTask);

        var kioskOrders = allOrdersTask.Result.Where(o => o.PosId == kiosk.PosId).ToList();
        var kioskInvoices = allInvoicesTask.Result.Where(i => i.PosId == kiosk.PosId).ToList();
        var yesterdayKiosk = yesterdayAllTask.Result.Where(o => o.PosId == kiosk.PosId).ToList();

        var summary = BuildSummary(day, kioskOrders, kioskInvoices, yesterdayKiosk);
        summary.KioskName = kiosk.DisplayName;
        summary.KioskId = kiosk.Id.ToString();
        summary.KioskColor = kiosk.Color;
        summary.KioskIcon = kiosk.Icon;
        return summary;
    }



    /// <summary>
    /// Summary con desglose por cada kiosko.
    /// </summary>
    public async Task<MultiKioskSummaryDto> GetMultiKioskSummaryAsync(DateTime businessDay)
    {
        var day = DateOnly.FromDateTime(businessDay);
        var kiosks = (await _kioskRepo.GetListAsync()).Where(k => k.IsActive).OrderBy(k => k.SortOrder).ToList();

        var allOrders = await _posService.GetSalesOrdersAsync(day, includeProcessed: true);
        var allInvoices = await _posService.GetInvoicesAsync(day, includeProcessed: true);

        var result = new MultiKioskSummaryDto
        {
            BusinessDay = day.ToString("yyyy-MM-dd"),
            // Totales globales
            GlobalSummary = BuildSummary(day, allOrders, allInvoices),
            // Desglose por kiosko
            KioskSummaries = kiosks.Select(k =>
            {
                // ⚠️ CAMBIO AQUÍ: Filtramos por PosId
                var kOrders = allOrders.Where(o => o.PosId == k.PosId).ToList();
                var kInvoices = allInvoices.Where(i => i.PosId == k.PosId).ToList();
                var s = BuildSummary(day, kOrders, kInvoices);
                s.KioskId = k.Id.ToString();
                s.KioskName = k.DisplayName;
                s.KioskColor = k.Color;
                s.KioskIcon = k.Icon;
                return s;
            }).ToList()
        };

        return result;
    }

    public async Task<DashboardRangeDto> GetRangeSummaryAsync(DateTime fromDate, DateTime toDate)
    {
        var result = new DashboardRangeDto
        {
            FromDate = fromDate.ToString("yyyy-MM-dd"),
            ToDate = toDate.ToString("yyyy-MM-dd"),
            DailySummaries = new List<DailySummaryDto>()
        };

        for (var date = fromDate.Date; date <= toDate.Date; date = date.AddDays(1))
        {
            var day = DateOnly.FromDateTime(date);
            var orders = await _posService.GetSalesOrdersAsync(day, includeProcessed: true);
            var revenue = orders.Sum(o => o.Totals.GrossAmount);
            result.DailySummaries.Add(new DailySummaryDto
            {
                BusinessDay = day.ToString("yyyy-MM-dd"),
                Revenue = revenue,
                OrderCount = orders.Count,
                AverageTicket = orders.Count > 0 ? revenue / orders.Count : 0
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

    public Task<bool> TestPosConnectionAsync() => _posService.TestConnectionAsync();

    // ═══ Builder privado ═══

    private static DashboardSummaryDto BuildSummary(
        DateOnly day,
        List<PosSalesOrder> orders,
        List<PosInvoice> invoices,
        List<PosSalesOrder>? yesterdayOrders = null)
    {
        var lines = orders.SelectMany(o => o.Lines)
            .Where(l => l.LineType != "MenuHeader" && l.Quantity > 0).ToList();
        var totalRevenue = orders.Sum(o => o.Totals.GrossAmount);
        var totalNet = orders.Sum(o => o.Totals.NetAmount);
        var totalVat = orders.Sum(o => o.Totals.VatAmount);
        var allPayments = orders.SelectMany(o => o.Payments).ToList();

        // Hora pico
        var hourlyGroups = orders.GroupBy(o => o.Date.Hour).ToList();
        var peakHourGroup = hourlyGroups.OrderByDescending(g => g.Sum(o => o.Totals.GrossAmount)).FirstOrDefault();
        var peakHour = peakHourGroup?.Key ?? 0;
        var peakHourRevenue = peakHourGroup?.Sum(o => o.Totals.GrossAmount) ?? 0;
        var peakHourOrders = peakHourGroup?.Count() ?? 0;

        // Comparativa con ayer
        decimal? revenueChange = null, ordersChange = null, ticketChange = null, itemsChange = null;
        decimal? yesterdayRevenue = null;
        int? yesterdayOrderCount = null;

        if (yesterdayOrders != null && yesterdayOrders.Count > 0)
        {
            var yRevenue = yesterdayOrders.Sum(o => o.Totals.GrossAmount);
            var yOrders = yesterdayOrders.Count;
            var yTicket = yOrders > 0 ? yRevenue / yOrders : 0;
            var yItems = yesterdayOrders.SelectMany(o => o.Lines)
                .Where(l => l.LineType != "MenuHeader" && l.Quantity > 0)
                .Sum(l => (int)Math.Abs(l.Quantity));

            yesterdayRevenue = yRevenue;
            yesterdayOrderCount = yOrders;

            if (yRevenue > 0)
                revenueChange = Math.Round((totalRevenue - yRevenue) / yRevenue * 100, 1);
            if (yOrders > 0)
                ordersChange = Math.Round((decimal)(orders.Count - yOrders) / yOrders * 100, 1);
            if (yTicket > 0)
            {
                var todayTicket = orders.Count > 0 ? totalRevenue / orders.Count : 0;
                ticketChange = Math.Round((todayTicket - yTicket) / yTicket * 100, 1);
            }
            if (yItems > 0)
            {
                var todayItems = lines.Sum(l => (int)Math.Abs(l.Quantity));
                itemsChange = Math.Round((decimal)(todayItems - yItems) / yItems * 100, 1);
            }
        }

        // Desglose Llevar vs Local
        var tableOrders = orders.Where(o => o.OrderType == PosOrderType.Table).ToList();
        var takeAwayOrd = orders.Where(o => o.OrderType == PosOrderType.TakeAway).ToList();
        var deliveryOrd = orders.Where(o => o.OrderType == PosOrderType.Delivery).ToList();

        return new DashboardSummaryDto
        {
            BusinessDay = day.ToString("yyyy-MM-dd"),
            TotalOrders = orders.Count,
            TotalInvoices = invoices.Count,
            TotalRevenue = totalRevenue,
            TotalNetRevenue = totalNet,
            VatCollected = totalVat,
            TotalItemsSold = lines.Sum(l => (int)Math.Abs(l.Quantity)),
            TotalGuests = orders.Sum(o => o.Guests ?? 1),
            AverageTicket = orders.Count > 0 ? Math.Round(totalRevenue / orders.Count, 2) : 0,

            // Hora pico
            PeakHour = peakHourGroup != null ? $"{peakHour:D2}:00 - {peakHour + 1:D2}:00" : "—",
            PeakHourRevenue = peakHourRevenue,
            PeakHourOrders = peakHourOrders,

            // Comparativa
            RevenueChangePercent = revenueChange,
            OrdersChangePercent = ordersChange,
            TicketChangePercent = ticketChange,
            ItemsChangePercent = itemsChange,
            YesterdayRevenue = yesterdayRevenue,
            YesterdayOrders = yesterdayOrderCount,

            // Tipo pedido
            TableOrders = tableOrders.Count,
            TakeAwayOrders = takeAwayOrd.Count,
            DeliveryOrders = deliveryOrd.Count,
            TableRevenue = tableOrders.Sum(o => o.Totals.GrossAmount),
            TakeAwayRevenue = takeAwayOrd.Sum(o => o.Totals.GrossAmount),
            DeliveryRevenue = deliveryOrd.Sum(o => o.Totals.GrossAmount),

            // Pagos
            PaymentBreakdown = allPayments
                .GroupBy(p => p.PaymentMethodName)
                .Select(g => new PaymentBreakdownDto
                {
                    PaymentMethodName = g.Key,
                    TransactionCount = g.Count(),
                    TotalAmount = g.Sum(p => p.Amount),
                    Percentage = totalRevenue > 0 ? Math.Round(g.Sum(p => p.Amount) / totalRevenue * 100, 2) : 0
                }).OrderByDescending(p => p.TotalAmount).ToList(),

            // Top productos
            TopProducts = lines
                .GroupBy(l => new { l.ProductId, l.ProductName, l.FamilyName })
                .Select(g => new TopProductDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    FamilyName = g.Key.FamilyName,
                    QuantitySold = (int)g.Sum(l => l.Quantity),
                    TotalRevenue = g.Sum(l => l.TotalAmount),
                    AveragePrice = g.Average(l => l.Price),
                    OrderAppearances = g.Count()
                }).OrderByDescending(p => p.QuantitySold).Take(10).ToList(),

            // Familias
            TopFamilies = lines.Where(l => l.FamilyId.HasValue)
                .GroupBy(l => new { l.FamilyId, l.FamilyName })
                .Select(g => new TopFamilyDto
                {
                    FamilyId = g.Key.FamilyId ?? 0,
                    FamilyName = g.Key.FamilyName ?? "Sin familia",
                    ItemsSold = (int)g.Sum(l => l.Quantity),
                    TotalRevenue = g.Sum(l => l.TotalAmount),
                    Percentage = totalRevenue > 0 ? Math.Round(g.Sum(l => l.TotalAmount) / totalRevenue * 100, 2) : 0
                }).OrderByDescending(f => f.TotalRevenue).ToList(),

            // Hourly (mejorado con ticket medio y items)
            HourlySales = Enumerable.Range(0, 24).Select(h =>
            {
                var hOrders = orders.Where(o => o.Date.Hour == h).ToList();
                var hRevenue = hOrders.Sum(o => o.Totals.GrossAmount);
                var hItems = hOrders.SelectMany(o => o.Lines)
                    .Where(l => l.LineType != "MenuHeader" && l.Quantity > 0)
                    .Sum(l => (int)Math.Abs(l.Quantity));
                return new HourlySalesDto
                {
                    Hour = h,
                    HourLabel = $"{h:D2}:00",
                    OrderCount = hOrders.Count,
                    Revenue = hRevenue,
                    AverageTicket = hOrders.Count > 0 ? Math.Round(hRevenue / hOrders.Count, 2) : 0,
                    ItemCount = hItems
                };
            }).ToList()
        };
    }

}
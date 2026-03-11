using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using KioskPos.Analytics.Analytics.Entities;
using KioskPos.Analytics.PosIntegration;
using KioskPos.Analytics.PosIntegration.Interfaces;
using KioskPos.Analytics.PosIntegration.Models;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace KioskPos.Analytics.BackgroundJobs;

/// <summary>
/// Argumento para el job de sincronización diaria.
/// </summary>
[Serializable]
public class DailySalesSyncArgs
{
    public string BusinessDay { get; set; } = string.Empty;
    public Guid? PosConnectionConfigId { get; set; }
}

/// <summary>
/// Background Job que sincroniza las ventas de un día desde AgoraPOS
/// hacia la base de datos de Analytics (CachedSalesOrders, CachedInvoices, DashboardSnapshot).
/// </summary>
public class DailySalesSyncJob : AsyncBackgroundJob<DailySalesSyncArgs>, ITransientDependency
{
    private readonly IPosIntegrationService _posService;
    private readonly IRepository<CachedSalesOrder, Guid> _salesOrderRepo;
    private readonly IRepository<CachedInvoice, Guid> _invoiceRepo;
    private readonly IRepository<DashboardSnapshot, Guid> _snapshotRepo;
    private readonly IRepository<SyncHistory, Guid> _syncHistoryRepo;
    private readonly ILogger<DailySalesSyncJob> _logger;

    public DailySalesSyncJob(
        IPosIntegrationService posService,
        IRepository<CachedSalesOrder, Guid> salesOrderRepo,
        IRepository<CachedInvoice, Guid> invoiceRepo,
        IRepository<DashboardSnapshot, Guid> snapshotRepo,
        IRepository<SyncHistory, Guid> syncHistoryRepo,
        ILogger<DailySalesSyncJob> logger)
    {
        _posService = posService;
        _salesOrderRepo = salesOrderRepo;
        _invoiceRepo = invoiceRepo;
        _snapshotRepo = snapshotRepo;
        _syncHistoryRepo = syncHistoryRepo;
        _logger = logger;
    }

    [UnitOfWork]
    public override async Task ExecuteAsync(DailySalesSyncArgs args)
    {
        var startedAt = DateTime.UtcNow;
        var businessDay = DateOnly.Parse(args.BusinessDay);
        _logger.LogInformation("Iniciando sync de ventas para {BusinessDay}", args.BusinessDay);

        var syncHistory = new SyncHistory
        {
            PosConnectionConfigId = args.PosConnectionConfigId ?? Guid.Empty,
            Status = SyncStatus.InProgress,
            SyncType = "DailySales",
            BusinessDay = businessDay,
            StartedAt = startedAt
        };

        try
        {
            // 1. Obtener datos del POS
            var orders = await _posService.GetSalesOrdersAsync(businessDay, includeProcessed: true);
            var invoices = await _posService.GetInvoicesAsync(businessDay, includeProcessed: true);

            // 2. Limpiar datos anteriores del mismo día
            var existingOrders = await _salesOrderRepo.GetListAsync(x => x.BusinessDay == businessDay);
            if (existingOrders.Any())
                await _salesOrderRepo.DeleteManyAsync(existingOrders);

            var existingInvoices = await _invoiceRepo.GetListAsync(x => x.BusinessDay == businessDay);
            if (existingInvoices.Any())
                await _invoiceRepo.DeleteManyAsync(existingInvoices);

            // 3. Cachear SalesOrders
            var cachedOrders = orders.Select(o => new CachedSalesOrder
            {
                PosConnectionConfigId = args.PosConnectionConfigId ?? Guid.Empty,
                Serie = o.Serie,
                Number = o.Number,
                GlobalId = o.GlobalId,
                BusinessDay = businessDay,
                OrderDate = o.Date,
                Status = o.Status,
                OrderType = o.OrderType,
                GrossAmount = o.Totals.GrossAmount,
                NetAmount = o.Totals.NetAmount,
                VatAmount = o.Totals.VatAmount,
                ExternalPosId = o.PosId,
                ExternalUserId = o.UserId,
                UserName = o.UserName,
                ExternalCustomerId = o.CustomerId,
                ExternalSaleCenterId = o.SaleCenterId,
                SaleCenterName = o.SaleCenterName,
                SaleLocationName = o.SaleLocationName,
                Guests = o.Guests,
                PrimaryPaymentMethod = o.Payments.FirstOrDefault()?.PaymentMethodName,
                LinesJson = JsonSerializer.Serialize(o.Lines),
                PaymentsJson = JsonSerializer.Serialize(o.Payments),
                PosProcessedDate = o.ProcessedDate,
                LineCount = o.Lines.Count,
                ItemCount = o.Lines.Where(l => l.LineType != "MenuHeader").Sum(l => (int)Math.Abs(l.Quantity))
            }).ToList();

            if (cachedOrders.Any())
                await _salesOrderRepo.InsertManyAsync(cachedOrders);

            // 4. Cachear Invoices
            var cachedInvoices = invoices.Select(i => new CachedInvoice
            {
                PosConnectionConfigId = args.PosConnectionConfigId ?? Guid.Empty,
                Serie = i.Serie,
                Number = i.Number,
                GlobalId = i.GlobalId,
                BusinessDay = businessDay,
                InvoiceDate = i.Date,
                DocumentType = i.DocumentType,
                VatIncluded = i.VatIncluded,
                GrossAmount = i.Totals.GrossAmount,
                NetAmount = i.Totals.NetAmount,
                VatAmount = i.Totals.VatAmount,
                ExternalPosId = i.PosId,
                ExternalUserId = i.UserId,
                UserName = i.UserName,
                ExternalCustomerId = i.CustomerId,
                CustomerName = i.CustomerName,
                PrimaryPaymentMethod = i.Payments.FirstOrDefault()?.PaymentMethodName,
                LinesJson = JsonSerializer.Serialize(i.Lines),
                PaymentsJson = JsonSerializer.Serialize(i.Payments),
                PosProcessedDate = i.ProcessedDate,
                LineCount = i.Lines.Count
            }).ToList();

            if (cachedInvoices.Any())
                await _invoiceRepo.InsertManyAsync(cachedInvoices);

            // 5. Generar DashboardSnapshot
            await GenerateSnapshotAsync(businessDay, (List<PosSalesOrder>)orders, args.PosConnectionConfigId ?? Guid.Empty);

            // 6. Registrar éxito
            syncHistory.Status = SyncStatus.Completed;
            syncHistory.RecordsSynced = cachedOrders.Count + cachedInvoices.Count;
            syncHistory.CompletedAt = DateTime.UtcNow;
            syncHistory.Details = $"Orders: {cachedOrders.Count}, Invoices: {cachedInvoices.Count}";

            _logger.LogInformation(
                "Sync completado para {BusinessDay}: {Orders} pedidos, {Invoices} facturas",
                args.BusinessDay, cachedOrders.Count, cachedInvoices.Count);
        }
        catch (Exception ex)
        {
            syncHistory.Status = SyncStatus.Failed;
            syncHistory.ErrorMessage = ex.Message;
            syncHistory.CompletedAt = DateTime.UtcNow;
            _logger.LogError(ex, "Error en sync de ventas para {BusinessDay}", args.BusinessDay);
        }

        await _syncHistoryRepo.InsertAsync(syncHistory);
    }

    private async Task GenerateSnapshotAsync(
        DateOnly businessDay, List<PosSalesOrder> orders, Guid configId)
    {
        // Eliminar snapshot anterior
        var existing = await _snapshotRepo.FirstOrDefaultAsync(
            x => x.BusinessDay == businessDay && x.PosConnectionConfigId == configId);
        if (existing != null)
            await _snapshotRepo.DeleteAsync(existing);

        var lines = orders.SelectMany(o => o.Lines)
            .Where(l => l.LineType != "MenuHeader" && l.Quantity > 0).ToList();
        var totalRevenue = orders.Sum(o => o.Totals.GrossAmount);

        // Top 10 productos
        var topProducts = lines
            .GroupBy(l => new { l.ProductId, l.ProductName })
            .Select(g => new { g.Key.ProductName, Qty = (int)g.Sum(l => l.Quantity), Revenue = g.Sum(l => l.TotalAmount) })
            .OrderByDescending(p => p.Qty).Take(10).ToList();

        // Top familias
        var topFamilies = lines.Where(l => l.FamilyId.HasValue)
            .GroupBy(l => new { l.FamilyId, l.FamilyName })
            .Select(g => new { g.Key.FamilyName, Revenue = g.Sum(l => l.TotalAmount),
                Pct = totalRevenue > 0 ? Math.Round(g.Sum(l => l.TotalAmount) / totalRevenue * 100, 2) : 0 })
            .OrderByDescending(f => f.Revenue).ToList();

        // Ventas por hora
        var hourly = Enumerable.Range(0, 24).Select(h => new {
            Hour = h, Count = orders.Count(o => o.Date.Hour == h),
            Revenue = orders.Where(o => o.Date.Hour == h).Sum(o => o.Totals.GrossAmount)
        }).ToList();

        // Pagos
        var payments = orders.SelectMany(o => o.Payments)
            .GroupBy(p => p.PaymentMethodName)
            .Select(g => new { Name = g.Key, Count = g.Count(), Amount = g.Sum(p => p.Amount) })
            .OrderByDescending(p => p.Amount).ToList();

        var snapshot = new DashboardSnapshot
        {
            PosConnectionConfigId = configId,
            BusinessDay = businessDay,
            TotalRevenue = totalRevenue,
            TotalNetRevenue = orders.Sum(o => o.Totals.NetAmount),
            TotalOrders = orders.Count,
            TotalInvoices = 0,
            AverageTicket = orders.Count > 0 ? totalRevenue / orders.Count : 0,
            TotalItemsSold = lines.Sum(l => (int)l.Quantity),
            TotalGuests = orders.Sum(o => o.Guests ?? 1),
            TableOrders = orders.Count(o => o.OrderType == PosOrderType.Table),
            TakeAwayOrders = orders.Count(o => o.OrderType == PosOrderType.TakeAway),
            DeliveryOrders = orders.Count(o => o.OrderType == PosOrderType.Delivery),
            PaymentBreakdownJson = JsonSerializer.Serialize(payments),
            TopProductsJson = JsonSerializer.Serialize(topProducts),
            TopFamiliesJson = JsonSerializer.Serialize(topFamilies),
            HourlySalesJson = JsonSerializer.Serialize(hourly)
        };

        await _snapshotRepo.InsertAsync(snapshot);
    }
}

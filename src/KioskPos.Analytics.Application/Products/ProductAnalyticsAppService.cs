using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KioskPos.Analytics.PosIntegration.Interfaces;
using Volo.Abp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using KioskPos.Analytics.Permissions;

namespace KioskPos.Analytics.Products;

[Authorize(AnalyticsPermissions.Products.Default)]
public class ProductAnalyticsAppService : ApplicationService, IProductAnalyticsAppService
{
    private readonly IPosIntegrationService _posService;

    public ProductAnalyticsAppService(IPosIntegrationService posService)
    {
        _posService = posService;
    }

    public async Task<ProductAnalyticsSummaryDto> GetProductAnalyticsAsync(DateTime businessDay)
    {
        var day = DateOnly.FromDateTime(businessDay);
        var orders = await _posService.GetSalesOrdersAsync(day, includeProcessed: true);

        var lines = orders.SelectMany(o => o.Lines)
            .Where(l => l.LineType != "MenuHeader" && l.Quantity > 0)
            .ToList();

        var productGroups = lines
            .GroupBy(l => new { l.ProductId, l.ProductName, l.FamilyName })
            .Select(g => new ProductPerformanceDto
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.ProductName,
                FamilyName = g.Key.FamilyName,
                TotalQuantitySold = (int)g.Sum(l => l.Quantity),
                TotalRevenue = g.Sum(l => l.TotalAmount),
                AveragePrice = g.Average(l => l.Price),
                OrderAppearances = g.Count()
            })
            .ToList();

        return new ProductAnalyticsSummaryDto
        {
            BusinessDay = day.ToString("yyyy-MM-dd"),
            TotalProductsSold = (int)lines.Sum(l => l.Quantity),
            UniqueProducts = productGroups.Count,
            TopProducts = productGroups.OrderByDescending(p => p.TotalQuantitySold).Take(10).ToList(),
            BottomProducts = productGroups.OrderBy(p => p.TotalQuantitySold).Take(5).ToList(),
            FamilyBreakdown = BuildFamilyBreakdown(lines)
        };
    }

    public async Task<List<ProductPerformanceDto>> GetTopProductsAsync(DateTime businessDay, int count = 10)
    {
        var analytics = await GetProductAnalyticsAsync(businessDay);
        return analytics.TopProducts.Take(count).ToList();
    }

    public async Task<List<FamilyPerformanceDto>> GetFamilyBreakdownAsync(DateTime businessDay)
    {
        var day = DateOnly.FromDateTime(businessDay);
        var orders = await _posService.GetSalesOrdersAsync(day, includeProcessed: true);
        var lines = orders.SelectMany(o => o.Lines)
            .Where(l => l.LineType != "MenuHeader" && l.Quantity > 0)
            .ToList();
        return BuildFamilyBreakdown(lines);
    }

    public async Task<ProductPerformanceDto> GetProductDetailAsync(int productId, DateTime businessDay)
    {
        var day = DateOnly.FromDateTime(businessDay);
        var orders = await _posService.GetSalesOrdersAsync(day, includeProcessed: true);

        var lines = orders.SelectMany(o => o.Lines)
            .Where(l => l.ProductId == productId && l.Quantity > 0)
            .ToList();

        if (!lines.Any())
            throw new Volo.Abp.BusinessException("ProductNotFound");

        return new ProductPerformanceDto
        {
            ProductId = productId,
            ProductName = lines.First().ProductName,
            FamilyName = lines.First().FamilyName,
            TotalQuantitySold = (int)lines.Sum(l => l.Quantity),
            TotalRevenue = lines.Sum(l => l.TotalAmount),
            AveragePrice = lines.Average(l => l.Price),
            OrderAppearances = lines.Count
        };
    }

    private static List<FamilyPerformanceDto> BuildFamilyBreakdown(
        List<PosIntegration.Models.PosInvoiceLine> lines)
    {
        var totalRevenue = lines.Sum(l => l.TotalAmount);

        return lines
            .Where(l => l.FamilyId.HasValue)
            .GroupBy(l => new { l.FamilyId, l.FamilyName })
            .Select(g => new FamilyPerformanceDto
            {
                FamilyId = g.Key.FamilyId ?? 0,
                FamilyName = g.Key.FamilyName ?? "Sin familia",
                TotalQuantitySold = (int)g.Sum(l => l.Quantity),
                TotalRevenue = g.Sum(l => l.TotalAmount),
                Percentage = totalRevenue > 0 ? Math.Round(g.Sum(l => l.TotalAmount) / totalRevenue * 100, 2) : 0,
                TopProducts = g.GroupBy(l => new { l.ProductId, l.ProductName })
                    .Select(pg => new ProductPerformanceDto
                    {
                        ProductId = pg.Key.ProductId,
                        ProductName = pg.Key.ProductName,
                        TotalQuantitySold = (int)pg.Sum(l => l.Quantity),
                        TotalRevenue = pg.Sum(l => l.TotalAmount)
                    })
                    .OrderByDescending(p => p.TotalQuantitySold)
                    .Take(5)
                    .ToList()
            })
            .OrderByDescending(f => f.TotalRevenue)
            .ToList();
    }
}

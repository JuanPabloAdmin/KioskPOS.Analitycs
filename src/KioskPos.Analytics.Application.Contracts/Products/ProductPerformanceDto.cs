using System.Collections.Generic;

namespace KioskPos.Analytics.Products;

public class ProductPerformanceDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? FamilyName { get; set; }
    public int TotalQuantitySold { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal? CostPrice { get; set; }
    public decimal? Margin { get; set; }
    public decimal? MarginPercentage { get; set; }
    public int OrderAppearances { get; set; }
}

public class FamilyPerformanceDto
{
    public int FamilyId { get; set; }
    public string FamilyName { get; set; } = string.Empty;
    public int ProductCount { get; set; }
    public int TotalQuantitySold { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal Percentage { get; set; }
    public List<ProductPerformanceDto> TopProducts { get; set; } = new();
}

public class ProductAnalyticsSummaryDto
{
    public string BusinessDay { get; set; } = string.Empty;
    public int TotalProductsSold { get; set; }
    public int UniqueProducts { get; set; }
    public List<ProductPerformanceDto> TopProducts { get; set; } = new();
    public List<ProductPerformanceDto> BottomProducts { get; set; } = new();
    public List<FamilyPerformanceDto> FamilyBreakdown { get; set; } = new();
}

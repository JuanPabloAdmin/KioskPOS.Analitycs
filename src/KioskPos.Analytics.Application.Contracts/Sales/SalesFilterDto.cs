using System;

namespace KioskPos.Analytics.Sales;

public class SalesFilterDto
{
    public DateTime BusinessDay { get; set; } = DateTime.Today;
    public string? OrderType { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Status { get; set; }
    public string? SearchTerm { get; set; }
    public int MaxResultCount { get; set; } = 50;
    public int SkipCount { get; set; } = 0;
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; } = true;
}

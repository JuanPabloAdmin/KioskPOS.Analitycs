using System.Collections.Generic;

namespace KioskPos.Analytics.Sales;

public class SalesListResultDto<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public decimal TotalRevenue { get; set; }
}

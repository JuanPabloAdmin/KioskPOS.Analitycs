using System.Collections.Generic;
namespace KioskPos.Analytics.Dashboard;

public class MultiKioskSummaryDto
{
    public string BusinessDay { get; set; } = string.Empty;
    public DashboardSummaryDto GlobalSummary { get; set; } = new();
    public List<DashboardSummaryDto> KioskSummaries { get; set; } = new();
}

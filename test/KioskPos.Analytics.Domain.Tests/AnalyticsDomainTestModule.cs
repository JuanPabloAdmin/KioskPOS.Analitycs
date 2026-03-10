using Volo.Abp.Modularity;

namespace KioskPos.Analytics;

[DependsOn(
    typeof(AnalyticsDomainModule),
    typeof(AnalyticsTestBaseModule)
)]
public class AnalyticsDomainTestModule : AbpModule
{

}

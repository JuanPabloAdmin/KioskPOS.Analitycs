using Volo.Abp.Modularity;

namespace KioskPos.Analytics;

[DependsOn(
    typeof(AnalyticsApplicationModule),
    typeof(AnalyticsDomainTestModule)
)]
public class AnalyticsApplicationTestModule : AbpModule
{

}

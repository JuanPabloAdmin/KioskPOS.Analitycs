using Volo.Abp.Modularity;

namespace KioskPos.Analytics;

public abstract class AnalyticsApplicationTestBase<TStartupModule> : AnalyticsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}

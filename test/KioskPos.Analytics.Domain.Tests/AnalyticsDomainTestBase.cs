using Volo.Abp.Modularity;

namespace KioskPos.Analytics;

/* Inherit from this class for your domain layer tests. */
public abstract class AnalyticsDomainTestBase<TStartupModule> : AnalyticsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}

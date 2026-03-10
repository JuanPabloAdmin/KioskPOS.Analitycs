using KioskPos.Analytics.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace KioskPos.Analytics.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AnalyticsEntityFrameworkCoreModule),
    typeof(AnalyticsApplicationContractsModule)
    )]
public class AnalyticsDbMigratorModule : AbpModule
{
}

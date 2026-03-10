using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace KioskPos.Analytics.MongoDB;

[DependsOn(
    typeof(AnalyticsApplicationTestModule),
    typeof(AnalyticsMongoDbModule)
)]
public class AnalyticsMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = AnalyticsMongoDbFixture.GetRandomConnectionString();
        });
    }
}

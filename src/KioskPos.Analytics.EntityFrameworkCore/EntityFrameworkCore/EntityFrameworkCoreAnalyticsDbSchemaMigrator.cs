using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using KioskPos.Analytics.Data;
using Volo.Abp.DependencyInjection;

namespace KioskPos.Analytics.EntityFrameworkCore;

public class EntityFrameworkCoreAnalyticsDbSchemaMigrator
    : IAnalyticsDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreAnalyticsDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the AnalyticsDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<AnalyticsDbContext>()
            .Database
            .MigrateAsync();
    }
}

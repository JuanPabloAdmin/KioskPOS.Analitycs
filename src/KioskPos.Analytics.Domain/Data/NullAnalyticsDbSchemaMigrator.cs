using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace KioskPos.Analytics.Data;

/* This is used if database provider does't define
 * IAnalyticsDbSchemaMigrator implementation.
 */
public class NullAnalyticsDbSchemaMigrator : IAnalyticsDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}

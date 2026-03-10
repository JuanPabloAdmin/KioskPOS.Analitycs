using System.Threading.Tasks;

namespace KioskPos.Analytics.Data;

public interface IAnalyticsDbSchemaMigrator
{
    Task MigrateAsync();
}

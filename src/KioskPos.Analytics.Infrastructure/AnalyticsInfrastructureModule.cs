using KioskPos.Analytics.Infrastructure.PosProviders.Agora;
using KioskPos.Analytics.PosIntegration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace KioskPos.Analytics.Infrastructure;

[DependsOn(typeof(AnalyticsDomainModule))]
public class AnalyticsInfrastructureModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        // Registrar el HttpClient para AgoraApiClient
        context.Services.AddHttpClient<AgoraApiClient>();

        // Registrar el adapter de Agora como implementación de IPosIntegrationService
        // Para cambiar de POS en el futuro, solo cambia esta línea.
        context.Services.AddTransient<IPosIntegrationService>(provider =>
        {
            var client = provider.GetRequiredService<AgoraApiClient>();
            var logger = provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<AgoraPosIntegrationService>>();

            // Configurar con valores de appsettings.json
            var baseUrl = configuration["PosIntegration:Agora:BaseUrl"] ?? "http://localhost:8984";
            var apiToken = configuration["PosIntegration:Agora:ApiToken"] ?? "";
            client.Configure(baseUrl, apiToken);

            return new AgoraPosIntegrationService(client, logger);
        });
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace KioskPos.Analytics.BackgroundJobs;

/// <summary>
/// Worker que se ejecuta periódicamente para sincronizar las ventas del día actual.
/// Se ejecuta cada 15 minutos.
/// </summary>
public class AutoSyncWorker : AsyncPeriodicBackgroundWorkerBase
{
    public AutoSyncWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory)
        : base(timer, serviceScopeFactory)
    {
        Timer.Period = 15 * 60 * 1000; // 15 minutos en milisegundos
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        var logger = workerContext.ServiceProvider.GetRequiredService<ILogger<AutoSyncWorker>>();
        var jobManager = workerContext.ServiceProvider.GetRequiredService<IBackgroundJobManager>();

        logger.LogInformation("AutoSync: Encolando sync para {Today}", DateTime.Today.ToString("yyyy-MM-dd"));

        await jobManager.EnqueueAsync(new DailySalesSyncArgs
        {
            BusinessDay = DateTime.Today.ToString("yyyy-MM-dd")
        });
    }
}

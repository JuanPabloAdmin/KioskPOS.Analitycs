using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KioskPos.Analytics.Analytics.Entities;
using KioskPos.Analytics.BackgroundJobs;
using KioskPos.Analytics.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Repositories;

namespace KioskPos.Analytics.Sync;

[Authorize(AnalyticsPermissions.PosConfig.Sync)]
public class SyncAppService : ApplicationService, ISyncAppService
{
    private readonly IBackgroundJobManager _jobManager;
    private readonly IRepository<SyncHistory, Guid> _syncHistoryRepo;

    public SyncAppService(
        IBackgroundJobManager jobManager,
        IRepository<SyncHistory, Guid> syncHistoryRepo)
    {
        _jobManager = jobManager;
        _syncHistoryRepo = syncHistoryRepo;
    }

    public async Task<SyncResultDto> TriggerTodaySyncAsync()
    {
        return await TriggerDailySyncAsync(DateTime.Today);
    }

    public async Task<SyncResultDto> TriggerDailySyncAsync(DateTime businessDay)
    {
        var dayStr = businessDay.ToString("yyyy-MM-dd");

        await _jobManager.EnqueueAsync(new DailySalesSyncArgs
        {
            BusinessDay = dayStr
        });

        return new SyncResultDto
        {
            Queued = true,
            BusinessDay = dayStr,
            Message = $"Sincronización encolada para {dayStr}"
        };
    }

    public async Task<List<SyncHistoryDto>> GetSyncHistoryAsync(int maxResults = 20)
    {
        var query = await _syncHistoryRepo.GetQueryableAsync();
        var history = query
            .OrderByDescending(h => h.StartedAt)
            .Take(maxResults)
            .ToList();

        return history.Select(h => new SyncHistoryDto
        {
            Id = h.Id,
            SyncType = h.SyncType,
            BusinessDay = h.BusinessDay?.ToString("yyyy-MM-dd"),
            Status = h.Status.ToString(),
            RecordsSynced = h.RecordsSynced,
            StartedAt = h.StartedAt,
            CompletedAt = h.CompletedAt,
            ErrorMessage = h.ErrorMessage,
            Details = h.Details,
            DurationSeconds = h.CompletedAt.HasValue
                ? (h.CompletedAt.Value - h.StartedAt).TotalSeconds
                : null
        }).ToList();
    }
}

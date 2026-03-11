using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace KioskPos.Analytics.Sync;

public interface ISyncAppService : IApplicationService
{
    Task<SyncResultDto> TriggerDailySyncAsync(DateTime businessDay);
    Task<SyncResultDto> TriggerTodaySyncAsync();
    Task<List<SyncHistoryDto>> GetSyncHistoryAsync(int maxResults = 20);
}

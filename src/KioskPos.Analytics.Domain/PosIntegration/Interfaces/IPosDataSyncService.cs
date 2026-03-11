using KioskPos.Analytics.PosIntegration.Models;

namespace KioskPos.Analytics.PosIntegration.Interfaces;

/// <summary>
/// Servicio de sincronización de datos desde el POS hacia la DB de Analytics.
/// Usa IPosIntegrationService internamente para obtener los datos.
/// </summary>
public interface IPosDataSyncService
{
    Task<PosSyncResult> SyncMasterDataAsync(CancellationToken ct = default);
    Task<PosSyncResult> SyncDailySalesAsync(DateOnly businessDay, CancellationToken ct = default);
    Task<PosSyncResult> RunIncrementalSyncAsync(CancellationToken ct = default);
}

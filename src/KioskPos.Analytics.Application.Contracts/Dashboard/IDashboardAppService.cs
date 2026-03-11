using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace KioskPos.Analytics.Dashboard;

public interface IDashboardAppService : IApplicationService
{
    /// <summary>
    /// Obtiene el resumen del dashboard para un día específico.
    /// </summary>
    Task<DashboardSummaryDto> GetDailySummaryAsync(DateTime businessDay);

    /// <summary>
    /// Obtiene el resumen del dashboard para hoy.
    /// </summary>
    Task<DashboardSummaryDto> GetTodaySummaryAsync();

    /// <summary>
    /// Obtiene analíticas para un rango de fechas.
    /// </summary>
    Task<DashboardRangeDto> GetRangeSummaryAsync(DateTime fromDate, DateTime toDate);

    /// <summary>
    /// Test de conexión con el POS.
    /// </summary>
    Task<bool> TestPosConnectionAsync();
}

using KioskPos.Analytics.PosIntegration.Models;

namespace KioskPos.Analytics.PosIntegration.Interfaces;

/// <summary>
/// Interfaz principal de integración POS.
/// TODAS las implementaciones de POS (Agora, Square, Toast, etc.) 
/// deben implementar esta interfaz.
/// 
/// El core de Analytics NUNCA conoce los detalles del POS específico.
/// Solo usa esta interfaz para obtener datos normalizados.
/// </summary>
public interface IPosIntegrationService
{
    // ── Datos Maestros ──
    Task<List<PosProduct>> GetProductsAsync(CancellationToken ct = default);
    Task<List<PosFamily>> GetFamiliesAsync(CancellationToken ct = default);
    Task<List<PosPaymentMethod>> GetPaymentMethodsAsync(CancellationToken ct = default);
    Task<List<PosVat>> GetVatsAsync(CancellationToken ct = default);
    Task<List<PosSaleCenter>> GetSaleCentersAsync(CancellationToken ct = default);
    Task<List<PosUser>> GetUsersAsync(CancellationToken ct = default);
    Task<List<PosCustomer>> GetCustomersAsync(CancellationToken ct = default);

    // ── Datos de Ventas (por fecha de negocio) ──
    Task<List<PosInvoice>> GetInvoicesAsync(DateOnly businessDay, bool includeProcessed = false, CancellationToken ct = default);
    Task<List<PosSalesOrder>> GetSalesOrdersAsync(DateOnly businessDay, bool includeProcessed = false, CancellationToken ct = default);
    Task<List<PosCashCloseOut>> GetCashCloseOutsAsync(DateOnly businessDay, CancellationToken ct = default);

    // ── Consultas Personalizadas ──
    Task<PosCustomQueryResult> ExecuteCustomQueryAsync(PosCustomQueryRequest request, CancellationToken ct = default);

    // ── Control de Procesamiento ──
    Task<bool> MarkDocumentsAsProcessedAsync(List<(string Serie, int Number)> documents, CancellationToken ct = default);

    // ── Diagnóstico ──
    Task<bool> TestConnectionAsync(CancellationToken ct = default);
}

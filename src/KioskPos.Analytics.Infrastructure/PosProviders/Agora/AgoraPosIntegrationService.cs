using KioskPos.Analytics.PosIntegration.Interfaces;
using KioskPos.Analytics.PosIntegration.Models;
using KioskPos.Analytics.Infrastructure.PosProviders.Agora.Models;
using KioskPos.Analytics.Infrastructure.PosProviders.Agora.Mappers;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace KioskPos.Analytics.Infrastructure.PosProviders.Agora;

/// <summary>
/// Implementación concreta de IPosIntegrationService para AgoraPOS.
/// Este es el ADAPTER que traduce las llamadas genéricas a la API específica de Agora.
/// </summary>
public class AgoraPosIntegrationService : IPosIntegrationService, ITransientDependency
{
    private readonly AgoraApiClient _client;
    private readonly ILogger<AgoraPosIntegrationService> _logger;

    public AgoraPosIntegrationService(
        AgoraApiClient client,
        ILogger<AgoraPosIntegrationService> logger)
    {
        _client = client;
        _logger = logger;
    }

    // ── Datos Maestros ──

    public async Task<List<PosProduct>> GetProductsAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Obteniendo productos desde Agora...");
        var response = await _client.GetAsync<AgoraMasterDataResponse>(
            "api/export-master?filter=Products", ct);
        return response?.Products?
            .Where(p => p.DeletionDate == null)
            .Select(AgoraMappers.ToPos)
            .ToList() ?? new();
    }

    public async Task<List<PosFamily>> GetFamiliesAsync(CancellationToken ct = default)
    {
        var response = await _client.GetAsync<AgoraMasterDataResponse>(
            "api/export-master?filter=Families", ct);
        return response?.Families?
            .Where(f => f.DeletionDate == null)
            .Select(AgoraMappers.ToPos)
            .ToList() ?? new();
    }

    public async Task<List<PosPaymentMethod>> GetPaymentMethodsAsync(CancellationToken ct = default)
    {
        var response = await _client.GetAsync<AgoraMasterDataResponse>(
            "api/export-master?filter=PaymentMethods", ct);
        return response?.PaymentMethods?
            .Where(pm => pm.DeletionDate == null)
            .Select(AgoraMappers.ToPos)
            .ToList() ?? new();
    }

    public async Task<List<PosVat>> GetVatsAsync(CancellationToken ct = default)
    {
        var response = await _client.GetAsync<AgoraMasterDataResponse>(
            "api/export-master?filter=Vats", ct);
        return response?.Vats?.Select(AgoraMappers.ToPos).ToList() ?? new();
    }

    public async Task<List<PosSaleCenter>> GetSaleCentersAsync(CancellationToken ct = default)
    {
        var response = await _client.GetAsync<AgoraMasterDataResponse>(
            "api/export-master?filter=SaleCenters", ct);
        return response?.SaleCenters?
            .Where(sc => sc.DeletionDate == null)
            .Select(AgoraMappers.ToPos)
            .ToList() ?? new();
    }

    public async Task<List<PosUser>> GetUsersAsync(CancellationToken ct = default)
    {
        var response = await _client.GetAsync<AgoraMasterDataResponse>(
            "api/export-master?filter=Users", ct);
        return response?.Users?
            .Where(u => u.DeletionDate == null)
            .Select(AgoraMappers.ToPos)
            .ToList() ?? new();
    }

    public async Task<List<PosCustomer>> GetCustomersAsync(CancellationToken ct = default)
    {
        var response = await _client.GetAsync<AgoraMasterDataResponse>(
            "api/export-master?filter=Customers", ct);
        return response?.Customers?
            .Where(c => c.DeletionDate == null)
            .Select(AgoraMappers.ToPos)
            .ToList() ?? new();
    }

    // ── Datos de Ventas ──

    public async Task<List<PosInvoice>> GetInvoicesAsync(
        DateOnly businessDay, bool includeProcessed = false, CancellationToken ct = default)
    {
        var url = $"api/export?filter=Invoices&business-day={businessDay:yyyy-MM-dd}";
        if (includeProcessed) url += "&include-processed=true";

        var response = await _client.GetAsync<AgoraExportResponse>(url, ct);
        return response?.Invoices?.Select(AgoraMappers.ToPos).ToList() ?? new();
    }

    public async Task<List<PosSalesOrder>> GetSalesOrdersAsync(
        DateOnly businessDay, bool includeProcessed = false, CancellationToken ct = default)
    {
        var url = $"api/export?filter=SalesOrders&business-day={businessDay:yyyy-MM-dd}";
        if (includeProcessed) url += "&include-processed=true";

        var response = await _client.GetAsync<AgoraExportResponse>(url, ct);
        return response?.SalesOrders?.Select(AgoraMappers.ToPos).ToList() ?? new();
    }

    public async Task<List<PosCashCloseOut>> GetCashCloseOutsAsync(
        DateOnly businessDay, CancellationToken ct = default)
    {
        var response = await _client.GetAsync<AgoraExportResponse>(
            $"api/export?filter=PosCloseOuts&business-day={businessDay:yyyy-MM-dd}", ct);
        return response?.PosCloseOuts?.Select(AgoraMappers.ToPos).ToList() ?? new();
    }

    // ── Custom Queries ──

    public async Task<PosCustomQueryResult> ExecuteCustomQueryAsync(
        PosCustomQueryRequest request, CancellationToken ct = default)
    {
        try
        {
            var body = new
            {
                QueryGuid = request.QueryIdentifier,
                Params = request.Parameters
            };
            var raw = await _client.PostRawAsync("api/custom-query", body, ct);
            return new PosCustomQueryResult { Success = true, RawJson = raw };
        }
        catch (Exception ex)
        {
            return new PosCustomQueryResult { Success = false, ErrorMessage = ex.Message };
        }
    }

    // ── Control de Procesamiento ──

    public async Task<bool> MarkDocumentsAsProcessedAsync(
        List<(string Serie, int Number)> documents, CancellationToken ct = default)
    {
        var body = documents.Select(d => new { Serie = d.Serie, Number = d.Number });
        try
        {
            await _client.PostRawAsync("api/doc/processed", body, ct);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marcando documentos como procesados en Agora");
            return false;
        }
    }

    // ── Diagnóstico ──

    public Task<bool> TestConnectionAsync(CancellationToken ct = default)
        => _client.TestConnectionAsync(ct);
}

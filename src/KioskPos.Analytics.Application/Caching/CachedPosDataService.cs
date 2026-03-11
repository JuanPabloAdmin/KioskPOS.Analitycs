using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KioskPos.Analytics.PosIntegration.Interfaces;
using KioskPos.Analytics.PosIntegration.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace KioskPos.Analytics.Caching;

/// <summary>
/// Wrapper con caché en memoria para datos maestros del POS.
/// Evita llamadas repetitivas a la API de Agora para productos, familias, etc.
/// TTL configurable por tipo de dato.
/// </summary>
public class CachedPosDataService : ITransientDependency
{
    private readonly IPosIntegrationService _posService;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CachedPosDataService> _logger;

    private static readonly TimeSpan ProductsCacheDuration = TimeSpan.FromHours(1);
    private static readonly TimeSpan FamiliesCacheDuration = TimeSpan.FromHours(2);
    private static readonly TimeSpan PaymentMethodsCacheDuration = TimeSpan.FromHours(4);
    private static readonly TimeSpan VatsCacheDuration = TimeSpan.FromHours(12);

    public CachedPosDataService(
        IPosIntegrationService posService,
        IMemoryCache cache,
        ILogger<CachedPosDataService> logger)
    {
        _posService = posService;
        _cache = cache;
        _logger = logger;
    }

    public async Task<List<PosProduct>> GetProductsAsync()
    {
        return await GetOrCreateAsync("pos:products", ProductsCacheDuration,
            () => _posService.GetProductsAsync());
    }

    public async Task<List<PosFamily>> GetFamiliesAsync()
    {
        return await GetOrCreateAsync("pos:families", FamiliesCacheDuration,
            () => _posService.GetFamiliesAsync());
    }

    public async Task<List<PosPaymentMethod>> GetPaymentMethodsAsync()
    {
        return await GetOrCreateAsync("pos:payment-methods", PaymentMethodsCacheDuration,
            () => _posService.GetPaymentMethodsAsync());
    }

    public async Task<List<PosVat>> GetVatsAsync()
    {
        return await GetOrCreateAsync("pos:vats", VatsCacheDuration,
            () => _posService.GetVatsAsync());
    }

    public async Task<List<PosSaleCenter>> GetSaleCentersAsync()
    {
        return await GetOrCreateAsync("pos:sale-centers", FamiliesCacheDuration,
            () => _posService.GetSaleCentersAsync());
    }

    /// <summary>
    /// Invalida toda la caché de datos maestros.
    /// Útil después de una sincronización o cambio de configuración.
    /// </summary>
    public void InvalidateAll()
    {
        _cache.Remove("pos:products");
        _cache.Remove("pos:families");
        _cache.Remove("pos:payment-methods");
        _cache.Remove("pos:vats");
        _cache.Remove("pos:sale-centers");
        _logger.LogInformation("Caché de datos maestros POS invalidada");
    }

    private async Task<T> GetOrCreateAsync<T>(string key, TimeSpan duration, Func<Task<T>> factory)
    {
        if (_cache.TryGetValue(key, out T? cached) && cached != null)
        {
            _logger.LogDebug("Cache HIT: {Key}", key);
            return cached;
        }

        _logger.LogDebug("Cache MISS: {Key} — obteniendo del POS", key);
        var data = await factory();
        _cache.Set(key, data, duration);
        return data;
    }
}

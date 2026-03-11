using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace KioskPos.Analytics.Infrastructure.PosProviders.Agora;

/// <summary>
/// Cliente HTTP para comunicarse con la API de AgoraPOS.
/// Encapsula la autenticación (Api-Token header) y serialización.
/// </summary>
public class AgoraApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AgoraApiClient> _logger;

    public AgoraApiClient(HttpClient httpClient, ILogger<AgoraApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public void Configure(string baseUrl, string apiToken)
    {
        _httpClient.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Api-Token", apiToken);
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    /// <summary>GET genérico que devuelve el JSON deserializado.</summary>
    public async Task<T?> GetAsync<T>(string endpoint, CancellationToken ct = default)
    {
        _logger.LogDebug("Agora GET: {Endpoint}", endpoint);
        var response = await _httpClient.GetAsync(endpoint, ct);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync(ct);
        return JsonSerializer.Deserialize<T>(json, _jsonOptions);
    }

    /// <summary>GET que devuelve el JSON crudo como string.</summary>
    public async Task<string> GetRawAsync(string endpoint, CancellationToken ct = default)
    {
        _logger.LogDebug("Agora GET (raw): {Endpoint}", endpoint);
        var response = await _httpClient.GetAsync(endpoint, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(ct);
    }

    /// <summary>POST genérico con body JSON.</summary>
    public async Task<T?> PostAsync<T>(string endpoint, object body, CancellationToken ct = default)
    {
        _logger.LogDebug("Agora POST: {Endpoint}", endpoint);
        var content = new StringContent(
            JsonSerializer.Serialize(body, _jsonOptions),
            Encoding.UTF8,
            "application/json");
        var response = await _httpClient.PostAsync(endpoint, content, ct);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync(ct);
        return JsonSerializer.Deserialize<T>(json, _jsonOptions);
    }

    /// <summary>POST que devuelve el JSON crudo.</summary>
    public async Task<string> PostRawAsync(string endpoint, object body, CancellationToken ct = default)
    {
        _logger.LogDebug("Agora POST (raw): {Endpoint}", endpoint);
        var content = new StringContent(
            JsonSerializer.Serialize(body, _jsonOptions),
            Encoding.UTF8,
            "application/json");
        var response = await _httpClient.PostAsync(endpoint, content, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(ct);
    }

    /// <summary>Test de conexión (intenta obtener familias como ping).</summary>
    public async Task<bool> TestConnectionAsync(CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync("api/export-master?filter=Families", ct);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Agora connection test failed");
            return false;
        }
    }

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}

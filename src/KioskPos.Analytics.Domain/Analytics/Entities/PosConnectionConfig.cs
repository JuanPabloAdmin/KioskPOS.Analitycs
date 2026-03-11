using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace KioskPos.Analytics.Analytics.Entities;

/// <summary>
/// Configuración de conexión a un proveedor POS.
/// Cada tenant/restaurante tiene su propia configuración.
/// </summary>
public class PosConnectionConfig : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }

    /// <summary>Tipo de POS: Agora, Square, etc.</summary>
    public PosIntegration.PosProviderType ProviderType { get; set; }

    /// <summary>Nombre descriptivo (ej: "Restaurante Centro")</summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>URL base del API del POS</summary>
    public string ApiBaseUrl { get; set; } = string.Empty;

    /// <summary>Token de autenticación del API</summary>
    public string ApiToken { get; set; } = string.Empty;

    /// <summary>URL del ACMS (solo Agora)</summary>
    public string? AcmsBaseUrl { get; set; }

    /// <summary>Token del ACMS (solo Agora)</summary>
    public string? AcmsApiToken { get; set; }

    /// <summary>ID del Workplace en el POS</summary>
    public string? WorkplaceId { get; set; }

    /// <summary>ID del POS/Terminal</summary>
    public string? PosId { get; set; }

    /// <summary>ID de la tarifa por defecto</summary>
    public string? DefaultPriceListId { get; set; }

    /// <summary>Identificador del kiosko</summary>
    public string? KioskIdentifier { get; set; }

    /// <summary>Indica si la conexión está activa</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Última fecha de sincronización exitosa</summary>
    public DateTime? LastSuccessfulSync { get; set; }
}

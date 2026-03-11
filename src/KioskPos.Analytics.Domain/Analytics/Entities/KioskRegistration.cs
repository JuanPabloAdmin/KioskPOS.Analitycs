using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace KioskPos.Analytics.Analytics.Entities;

/// <summary>
/// Registro de un kiosko individual dentro del mismo local.
/// Todos comparten la misma URL/Token de Agora (configurado en appsettings),
/// pero cada uno tiene su propio Workplace, Pos, User, SaleCenter.
/// </summary>
public class KioskRegistration : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }

    /// <summary>Nombre visible: "Kiosko Entrada", "Kiosko Terraza", etc.</summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>Id del Workplace en Agora (filtra las ventas)</summary>
    public int WorkplaceId { get; set; }

    /// <summary>Id del TPV (Pos) en Agora</summary>
    public int PosId { get; set; }

    /// <summary>Id del grupo de TPVs en Agora</summary>
    public int PosGroupId { get; set; }

    /// <summary>Id del centro de venta 'Aquí' en Agora</summary>
    public int SaleCenterAquiId { get; set; }

    /// <summary>Id del centro de venta 'Llevar' en Agora</summary>
    public int SaleCenterTakeAwayId { get; set; }

    /// <summary>Id del usuario kiosko en Agora</summary>
    public int UserId { get; set; }

    /// <summary>Id del cliente por defecto en Agora</summary>
    public int DefaultCustomerId { get; set; }

    /// <summary>Id del almacén en Agora</summary>
    public int WarehouseId { get; set; }

    /// <summary>Id de la tarifa en Agora</summary>
    public int PriceListId { get; set; }

    /// <summary>Color para identificar en el dashboard</summary>
    public string Color { get; set; } = "#6366f1";

    /// <summary>Icono (emoji o clase CSS)</summary>
    public string Icon { get; set; } = "🖥️";

    /// <summary>Activo o desactivado</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Orden de aparición en el selector</summary>
    public int SortOrder { get; set; }
}

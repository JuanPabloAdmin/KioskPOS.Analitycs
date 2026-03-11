using System;
using System.ComponentModel.DataAnnotations;

namespace KioskPos.Analytics.KioskManagement;

public class KioskDto
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public int WorkplaceId { get; set; }
    public int PosId { get; set; }
    public int PosGroupId { get; set; }
    public int SaleCenterAquiId { get; set; }
    public int SaleCenterTakeAwayId { get; set; }
    public int UserId { get; set; }
    public int DefaultCustomerId { get; set; }
    public int WarehouseId { get; set; }
    public int PriceListId { get; set; }
    public string Color { get; set; } = "#6366f1";
    public string Icon { get; set; } = "🖥️";
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
}

public class CreateUpdateKioskDto
{
    [Required]
    [StringLength(128)]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    public int WorkplaceId { get; set; }
    [Required]
    public int PosId { get; set; }
    [Required]
    public int PosGroupId { get; set; }
    public int SaleCenterAquiId { get; set; }
    public int SaleCenterTakeAwayId { get; set; }
    [Required]
    public int UserId { get; set; }
    public int DefaultCustomerId { get; set; } = 1;
    public int WarehouseId { get; set; }
    public int PriceListId { get; set; }
    public string Color { get; set; } = "#6366f1";
    public string Icon { get; set; } = "🖥️";
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
}

public class KioskConnectionTestDto
{
    public Guid KioskId { get; set; }
    public string KioskName { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int? OrderCountToday { get; set; }
}

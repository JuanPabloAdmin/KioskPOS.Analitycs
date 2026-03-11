using System;
using System.Collections.Generic;

namespace KioskPos.Analytics.PosConfig;

public class PosConfigDto
{
    public Guid Id { get; set; }
    public string ProviderType { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string ApiBaseUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? LastSuccessfulSync { get; set; }
    public string? KioskIdentifier { get; set; }
}

public class CreateUpdatePosConfigDto
{
    public string DisplayName { get; set; } = string.Empty;
    public string ApiBaseUrl { get; set; } = string.Empty;
    public string ApiToken { get; set; } = string.Empty;
    public string? AcmsBaseUrl { get; set; }
    public string? AcmsApiToken { get; set; }
    public string? WorkplaceId { get; set; }
    public string? PosId { get; set; }
    public string? DefaultPriceListId { get; set; }
    public string? KioskIdentifier { get; set; }
}

public class PosConnectionTestResultDto
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int? ProductCount { get; set; }
    public int? FamilyCount { get; set; }
}

public class PosMasterDataSummaryDto
{
    public int ProductCount { get; set; }
    public int FamilyCount { get; set; }
    public int PaymentMethodCount { get; set; }
    public int SaleCenterCount { get; set; }
    public int UserCount { get; set; }
    public int VatCount { get; set; }
    public List<PosFamilySummaryDto> Families { get; set; } = new();
}

public class PosFamilySummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? Order { get; set; }
}

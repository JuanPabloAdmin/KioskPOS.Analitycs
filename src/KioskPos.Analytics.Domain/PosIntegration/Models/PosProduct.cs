namespace KioskPos.Analytics.PosIntegration.Models;

/// <summary>
/// Representación genérica de un producto de cualquier sistema POS.
/// </summary>
public class PosProduct
{
    public int ExternalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? FamilyId { get; set; }
    public string? FamilyName { get; set; }
    public int VatId { get; set; }
    public decimal? VatRate { get; set; }
    public decimal Price { get; set; }
    public decimal? CostPrice { get; set; }
    public string? Barcode { get; set; }
    public string? PLU { get; set; }
    public int? PreparationTypeId { get; set; }
    public int? PreparationOrderId { get; set; }
    public bool IsMenu { get; set; }
    public bool IsDeleted { get; set; }
    public List<PosProductPrice> Prices { get; set; } = new();
    public List<PosProductAddin> Addins { get; set; } = new();
}

public class PosProductPrice
{
    public int PriceListId { get; set; }
    public decimal? MainPrice { get; set; }
    public decimal? AddinPrice { get; set; }
    public decimal? MenuItemPrice { get; set; }
}

public class PosProductAddin
{
    public int SaleFormatId { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
}

using System.Text.Json.Serialization;

namespace KioskPos.Analytics.Infrastructure.PosProviders.Agora.Models;

/// <summary>
/// Respuesta de GET /api/export-master/ de AgoraPOS.
/// Mapea la estructura JSON/XML descrita en la Guía del Integrador.
/// </summary>
public class AgoraMasterDataResponse
{
    public List<AgoraProduct>? Products { get; set; }
    public List<AgoraFamily>? Families { get; set; }
    public List<AgoraPaymentMethod>? PaymentMethods { get; set; }
    public List<AgoraVat>? Vats { get; set; }
    public List<AgoraSaleCenter>? SaleCenters { get; set; }
    public List<AgoraUser>? Users { get; set; }
    public List<AgoraCustomer>? Customers { get; set; }
    public List<AgoraMenu>? Menus { get; set; }
}

public class AgoraProduct
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? FamilyId { get; set; }
    public int? VatId { get; set; }
    public string? ButtonText { get; set; }
    public string? Color { get; set; }
    public string? Barcode { get; set; }
    public int? Order { get; set; }
    public string? PLU { get; set; }
    public int? BaseSaleFormatId { get; set; }
    public int? PreparationTypeId { get; set; }
    public int? PreparationOrderId { get; set; }
    public decimal? CostPrice { get; set; }
    public string? DeletionDate { get; set; }
    public bool? IsMenu { get; set; }
    public List<AgoraProductPrice>? Prices { get; set; }
    public List<AgoraCostPrice>? CostPrices { get; set; }
    public List<AgoraBarcode>? Barcodes { get; set; }
    public List<AgoraAddinRole>? AddinRoles { get; set; }
    public List<AgoraAdditionalSaleFormat>? AdditionalSaleFormats { get; set; }
}

public class AgoraProductPrice
{
    public int PriceListId { get; set; }
    public decimal? MainPrice { get; set; }
    public decimal? AddinPrice { get; set; }
    public decimal? MenuItemPrice { get; set; }
    public decimal? Price { get; set; }
}

public class AgoraCostPrice
{
    public int WarehouseId { get; set; }
    public decimal? CostPrice { get; set; }
}

public class AgoraBarcode { public string? Value { get; set; } }

public class AgoraAddinRole
{
    public string? Name { get; set; }
    public int? MinAddins { get; set; }
    public int? MaxAddins { get; set; }
    public List<AgoraAddin>? Addins { get; set; }
}

public class AgoraAddin { public int? AddinSaleFormatId { get; set; } }

public class AgoraAdditionalSaleFormat
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal? Ratio { get; set; }
}

public class AgoraFamily
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? ParentFamilyId { get; set; }
    public string? Color { get; set; }
    public int? Order { get; set; }
    public string? DeletionDate { get; set; }
}

public class AgoraPaymentMethod
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool? GiveChange { get; set; }
    public bool? IncludeInBalance { get; set; }
    public bool? IsValidForSale { get; set; }
    public string? DeletionDate { get; set; }
}

public class AgoraVat
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal VatRate { get; set; }
    public decimal SurchargeRate { get; set; }
    public bool? Enabled { get; set; }
}

public class AgoraSaleCenter
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int PriceListId { get; set; }
    public bool VatIncluded { get; set; }
    public List<AgoraSaleLocation>? SaleLocations { get; set; }
    public string? DeletionDate { get; set; }
}
public class AgoraSaleLocation { public string? Name { get; set; } }

public class AgoraUser
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Profile { get; set; }
    public string? DeletionDate { get; set; }
}

public class AgoraCustomer
{
    public int Id { get; set; }
    public string? FiscalName { get; set; }
    public string? BusinessName { get; set; }
    public string? Cif { get; set; }
    public string? CardNumber { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public string? DeletionDate { get; set; }
}

public class AgoraMenu
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? VatId { get; set; }
    public int? FamilyId { get; set; }
    public string? DeletionDate { get; set; }
}

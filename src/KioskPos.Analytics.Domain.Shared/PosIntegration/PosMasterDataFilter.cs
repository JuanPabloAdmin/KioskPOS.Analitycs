namespace KioskPos.Analytics.PosIntegration;

/// <summary>
/// Filtros para datos maestros del POS.
/// En Agora: Products, Families, Users, Customers, etc.
/// </summary>
[Flags]
public enum PosMasterDataFilter
{
    None = 0,
    Products = 1,
    Families = 2,
    Users = 4,
    Customers = 8,
    PaymentMethods = 16,
    Vats = 32,
    SaleCenters = 64,
    PriceLists = 128,
    Menus = 256,
    Offers = 512,
    Stocks = 1024,
    Warehouses = 2048,
    PredefinedNotes = 4096,
    All = Products | Families | Users | Customers | PaymentMethods | Vats | SaleCenters | PriceLists | Menus
}

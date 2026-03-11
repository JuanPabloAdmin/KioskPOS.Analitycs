namespace KioskPos.Analytics.PosIntegration.Models;

public class PosInvoiceLine
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int? SaleFormatId { get; set; }
    public string? SaleFormatName { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalAmount { get; set; }
    public int VatId { get; set; }
    public decimal VatRate { get; set; }
    public decimal? DiscountRate { get; set; }
    public decimal? CashDiscount { get; set; }
    public int? FamilyId { get; set; }
    public string? FamilyName { get; set; }
    public string? LineType { get; set; }
    public int? PriceListId { get; set; }
}

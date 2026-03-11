namespace KioskPos.Analytics.Infrastructure.PosProviders.Agora.Models;

/// <summary>
/// Respuesta de GET /api/export/ de AgoraPOS.
/// </summary>
public class AgoraExportResponse
{
    public List<AgoraInvoice>? Invoices { get; set; }
    public List<AgoraSalesOrder>? SalesOrders { get; set; }
    public List<AgoraPosCloseOut>? PosCloseOuts { get; set; }
}

public class AgoraInvoice
{
    public string? Serie { get; set; }
    public int Number { get; set; }
    public string? GlobalId { get; set; }
    public string? BusinessDay { get; set; }
    public DateTime Date { get; set; }
    public string? DocumentType { get; set; }
    public bool VatIncluded { get; set; }
    public AgoraDocRef? Pos { get; set; }
    public AgoraDocRef? User { get; set; }
    public AgoraDocRef? Customer { get; set; }
    public AgoraDocRef? Workplace { get; set; }
    public AgoraTotals? Totals { get; set; }
    public List<AgoraInvoiceLine>? Lines { get; set; }
    public List<AgoraPayment>? Payments { get; set; }
    public string? ProcessedDate { get; set; }
    public List<AgoraInvoiceItem>? InvoiceItems { get; set; }
}

public class AgoraSalesOrder
{
    public string? Serie { get; set; }
    public int Number { get; set; }
    public string? GlobalId { get; set; }
    public string? BusinessDay { get; set; }
    public DateTime Date { get; set; }
    public string? Status { get; set; }
    public string? Type { get; set; }
    public bool VatIncluded { get; set; }
    public int? Guests { get; set; }
    public AgoraDocRef? Pos { get; set; }
    public AgoraDocRef? User { get; set; }
    public AgoraDocRef? Customer { get; set; }
    public AgoraSaleCenterRef? SaleCenter { get; set; }
    public AgoraDocRef? Workplace { get; set; }
    public AgoraTotals? Totals { get; set; }
    public List<AgoraInvoiceLine>? Lines { get; set; }
    public List<AgoraPayment>? Payments { get; set; }
    public string? ProcessedDate { get; set; }
}

public class AgoraPosCloseOut
{
    public string? BusinessDay { get; set; }
    public DateTime Date { get; set; }
    public AgoraDocRef? Pos { get; set; }
    public AgoraDocRef? User { get; set; }
    public AgoraTotals? Totals { get; set; }
}

public class AgoraDocRef
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class AgoraSaleCenterRef
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
}

public class AgoraTotals
{
    public decimal GrossAmount { get; set; }
    public decimal NetAmount { get; set; }
    public decimal VatAmount { get; set; }
    public decimal SurchargeAmount { get; set; }
    public List<AgoraTax>? Taxes { get; set; }
}

public class AgoraTax
{
    public decimal VatRate { get; set; }
    public decimal SurchargeRate { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal NetAmount { get; set; }
    public decimal VatAmount { get; set; }
    public decimal SurchargeAmount { get; set; }
}

public class AgoraInvoiceLine
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public int? SaleFormatId { get; set; }
    public string? SaleFormatName { get; set; }
    public decimal Quantity { get; set; }
    public decimal ProductPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public int VatId { get; set; }
    public decimal VatRate { get; set; }
    public decimal? DiscountRate { get; set; }
    public decimal? CashDiscount { get; set; }
    public int? FamilyId { get; set; }
    public string? FamilyName { get; set; }
    public string? Type { get; set; }
    public int? PriceListId { get; set; }
}

public class AgoraPayment
{
    public int PaymentMethodId { get; set; }
    public string? PaymentMethodName { get; set; }
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal ChangeAmount { get; set; }
    public decimal TipAmount { get; set; }
    public DateTime Date { get; set; }
    public bool IsPrepayment { get; set; }
    public string? ExtraInformation { get; set; }
}

public class AgoraInvoiceItem
{
    public AgoraTotals? Totals { get; set; }
    public List<AgoraInvoiceLine>? Lines { get; set; }
    public List<AgoraPayment>? Payments { get; set; }
}

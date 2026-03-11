namespace KioskPos.Analytics.PosIntegration.Models;

public class PosInvoice
{
    public string Serie { get; set; } = string.Empty;
    public int Number { get; set; }
    public string? GlobalId { get; set; }
    public string? BusinessDay { get; set; }
    public DateTime Date { get; set; }
    public string? DocumentType { get; set; }
    public bool VatIncluded { get; set; }
    public int? PosId { get; set; }
    public string? PosName { get; set; }
    public int? UserId { get; set; }
    public string? UserName { get; set; }
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int? WorkplaceId { get; set; }
    public string? WorkplaceName { get; set; }
    public PosTotals Totals { get; set; } = new();
    public List<PosInvoiceLine> Lines { get; set; } = new();
    public List<PosPayment> Payments { get; set; } = new();
    public DateTime? ProcessedDate { get; set; }
}

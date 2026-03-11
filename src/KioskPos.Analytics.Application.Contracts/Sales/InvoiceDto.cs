using System;
using System.Collections.Generic;

namespace KioskPos.Analytics.Sales;

public class InvoiceDto
{
    public string Serie { get; set; } = string.Empty;
    public int Number { get; set; }
    public string DocumentId { get; set; } = string.Empty;
    public string? GlobalId { get; set; }
    public string BusinessDay { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? DocumentType { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal NetAmount { get; set; }
    public decimal VatAmount { get; set; }
    public string? UserName { get; set; }
    public string? CustomerName { get; set; }
    public string? PrimaryPaymentMethod { get; set; }
    public int LineCount { get; set; }
    public List<SalesOrderLineDto> Lines { get; set; } = new();
    public List<SalesPaymentDto> Payments { get; set; } = new();
}

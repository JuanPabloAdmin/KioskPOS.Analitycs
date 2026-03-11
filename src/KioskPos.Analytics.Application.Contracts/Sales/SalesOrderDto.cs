using System;
using System.Collections.Generic;

namespace KioskPos.Analytics.Sales;

public class SalesOrderDto
{
    public string Serie { get; set; } = string.Empty;
    public int Number { get; set; }
    public string DocumentId { get; set; } = string.Empty;
    public string? GlobalId { get; set; }
    public string BusinessDay { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? Status { get; set; }
    public string OrderType { get; set; } = string.Empty;
    public decimal GrossAmount { get; set; }
    public decimal NetAmount { get; set; }
    public decimal VatAmount { get; set; }
    public int? Guests { get; set; }
    public string? UserName { get; set; }
    public string? CustomerName { get; set; }
    public string? SaleCenterName { get; set; }
    public string? SaleLocationName { get; set; }
    public string? PrimaryPaymentMethod { get; set; }
    public int LineCount { get; set; }
    public List<SalesOrderLineDto> Lines { get; set; } = new();
    public List<SalesPaymentDto> Payments { get; set; } = new();
}

public class SalesOrderLineDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalAmount { get; set; }
    public string? FamilyName { get; set; }
    public string? LineType { get; set; }
}

public class SalesPaymentDto
{
    public string PaymentMethodName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal ChangeAmount { get; set; }
    public decimal TipAmount { get; set; }
}

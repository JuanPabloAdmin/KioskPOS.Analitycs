using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KioskPos.Analytics.PosIntegration.Interfaces;
using KioskPos.Analytics.PosIntegration.Models;
using Volo.Abp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using KioskPos.Analytics.Permissions;

namespace KioskPos.Analytics.Sales;

[Authorize(AnalyticsPermissions.Sales.Default)]
public class SalesAppService : ApplicationService, ISalesAppService
{
    private readonly IPosIntegrationService _posService;

    public SalesAppService(IPosIntegrationService posService)
    {
        _posService = posService;
    }

    public async Task<SalesListResultDto<SalesOrderDto>> GetSalesOrdersAsync(SalesFilterDto filter)
    {
        var day = DateOnly.FromDateTime(filter.BusinessDay);
        var orders = await _posService.GetSalesOrdersAsync(day, includeProcessed: true);

        // Aplicar filtros
        var query = orders.AsEnumerable();

        if (!string.IsNullOrEmpty(filter.OrderType))
            query = query.Where(o => o.OrderType.ToString().Equals(filter.OrderType, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(filter.Status))
            query = query.Where(o => o.Status != null && o.Status.Equals(filter.Status, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(filter.PaymentMethod))
            query = query.Where(o => o.Payments.Any(p =>
                p.PaymentMethodName.Contains(filter.PaymentMethod, StringComparison.OrdinalIgnoreCase)));

        if (!string.IsNullOrEmpty(filter.SearchTerm))
            query = query.Where(o =>
                (o.CustomerName ?? "").Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                (o.SaleLocationName ?? "").Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                o.Lines.Any(l => l.ProductName.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase)));

        // Ordenar
        query = filter.SortBy?.ToLowerInvariant() switch
        {
            "amount" => filter.SortDesc ? query.OrderByDescending(o => o.Totals.GrossAmount) : query.OrderBy(o => o.Totals.GrossAmount),
            "number" => filter.SortDesc ? query.OrderByDescending(o => o.Number) : query.OrderBy(o => o.Number),
            _ => filter.SortDesc ? query.OrderByDescending(o => o.Date) : query.OrderBy(o => o.Date)
        };

        var filtered = query.ToList();

        return new SalesListResultDto<SalesOrderDto>
        {
            TotalCount = filtered.Count,
            TotalRevenue = filtered.Sum(o => o.Totals.GrossAmount),
            Items = filtered
                .Skip(filter.SkipCount)
                .Take(filter.MaxResultCount)
                .Select(MapToDto)
                .ToList()
        };
    }

    public async Task<SalesListResultDto<InvoiceDto>> GetInvoicesAsync(SalesFilterDto filter)
    {
        var day = DateOnly.FromDateTime(filter.BusinessDay);
        var invoices = await _posService.GetInvoicesAsync(day, includeProcessed: true);

        var query = invoices.AsEnumerable();

        if (!string.IsNullOrEmpty(filter.SearchTerm))
            query = query.Where(i =>
                (i.CustomerName ?? "").Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                i.Lines.Any(l => l.ProductName.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase)));

        query = filter.SortDesc ? query.OrderByDescending(i => i.Date) : query.OrderBy(i => i.Date);

        var filtered = query.ToList();

        return new SalesListResultDto<InvoiceDto>
        {
            TotalCount = filtered.Count,
            TotalRevenue = filtered.Sum(i => i.Totals.GrossAmount),
            Items = filtered
                .Skip(filter.SkipCount)
                .Take(filter.MaxResultCount)
                .Select(MapToDto)
                .ToList()
        };
    }

    public async Task<SalesOrderDto> GetSalesOrderDetailAsync(string serie, int number, DateTime businessDay)
    {
        var day = DateOnly.FromDateTime(businessDay);
        var orders = await _posService.GetSalesOrdersAsync(day, includeProcessed: true);
        var order = orders.FirstOrDefault(o => o.Serie == serie && o.Number == number);
        return order != null ? MapToDto(order) : throw new Volo.Abp.BusinessException("OrderNotFound");
    }

    public async Task<InvoiceDto> GetInvoiceDetailAsync(string serie, int number, DateTime businessDay)
    {
        var day = DateOnly.FromDateTime(businessDay);
        var invoices = await _posService.GetInvoicesAsync(day, includeProcessed: true);
        var invoice = invoices.FirstOrDefault(i => i.Serie == serie && i.Number == number);
        return invoice != null ? MapToDto(invoice) : throw new Volo.Abp.BusinessException("InvoiceNotFound");
    }

    // ── Mappers ──

    private static SalesOrderDto MapToDto(PosSalesOrder src) => new()
    {
        Serie = src.Serie,
        Number = src.Number,
        DocumentId = $"{src.Serie}-{src.Number}",
        GlobalId = src.GlobalId,
        BusinessDay = src.BusinessDay ?? "",
        Date = src.Date,
        Status = src.Status,
        OrderType = src.OrderType.ToString(),
        GrossAmount = src.Totals.GrossAmount,
        NetAmount = src.Totals.NetAmount,
        VatAmount = src.Totals.VatAmount,
        Guests = src.Guests,
        UserName = src.UserName,
        CustomerName = src.CustomerName,
        SaleCenterName = src.SaleCenterName,
        SaleLocationName = src.SaleLocationName,
        PrimaryPaymentMethod = src.Payments.FirstOrDefault()?.PaymentMethodName,
        LineCount = src.Lines.Count,
        Lines = src.Lines.Select(l => new SalesOrderLineDto
        {
            ProductId = l.ProductId,
            ProductName = l.ProductName,
            Quantity = l.Quantity,
            Price = l.Price,
            TotalAmount = l.TotalAmount,
            FamilyName = l.FamilyName,
            LineType = l.LineType
        }).ToList(),
        Payments = src.Payments.Select(p => new SalesPaymentDto
        {
            PaymentMethodName = p.PaymentMethodName,
            Amount = p.Amount,
            PaidAmount = p.PaidAmount,
            ChangeAmount = p.ChangeAmount,
            TipAmount = p.TipAmount
        }).ToList()
    };

    private static InvoiceDto MapToDto(PosInvoice src) => new()
    {
        Serie = src.Serie,
        Number = src.Number,
        DocumentId = $"{src.Serie}-{src.Number}",
        GlobalId = src.GlobalId,
        BusinessDay = src.BusinessDay ?? "",
        Date = src.Date,
        DocumentType = src.DocumentType,
        GrossAmount = src.Totals.GrossAmount,
        NetAmount = src.Totals.NetAmount,
        VatAmount = src.Totals.VatAmount,
        UserName = src.UserName,
        CustomerName = src.CustomerName,
        PrimaryPaymentMethod = src.Payments.FirstOrDefault()?.PaymentMethodName,
        LineCount = src.Lines.Count,
        Lines = src.Lines.Select(l => new SalesOrderLineDto
        {
            ProductId = l.ProductId,
            ProductName = l.ProductName,
            Quantity = l.Quantity,
            Price = l.Price,
            TotalAmount = l.TotalAmount,
            FamilyName = l.FamilyName,
            LineType = l.LineType
        }).ToList(),
        Payments = src.Payments.Select(p => new SalesPaymentDto
        {
            PaymentMethodName = p.PaymentMethodName,
            Amount = p.Amount,
            PaidAmount = p.PaidAmount,
            ChangeAmount = p.ChangeAmount,
            TipAmount = p.TipAmount
        }).ToList()
    };
}

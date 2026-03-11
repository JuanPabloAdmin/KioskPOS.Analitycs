using KioskPos.Analytics.PosIntegration;
using KioskPos.Analytics.PosIntegration.Models;
using KioskPos.Analytics.Infrastructure.PosProviders.Agora.Models;

namespace KioskPos.Analytics.Infrastructure.PosProviders.Agora.Mappers;

/// <summary>
/// Mappers estáticos de DTOs de Agora → DTOs genéricos POS.
/// Toda la lógica de transformación específica de Agora está aquí.
/// </summary>
public static class AgoraMappers
{
    public static PosProduct ToPos(AgoraProduct src) => new()
    {
        ExternalId = src.Id,
        Name = src.Name ?? string.Empty,
        FamilyId = src.FamilyId,
        VatId = src.VatId ?? 0,
        Price = src.Prices?.FirstOrDefault()?.MainPrice ?? src.Prices?.FirstOrDefault()?.Price ?? 0,
        CostPrice = src.CostPrice ?? src.CostPrices?.FirstOrDefault()?.CostPrice,
        Barcode = src.Barcode ?? src.Barcodes?.FirstOrDefault()?.Value,
        PLU = src.PLU,
        PreparationTypeId = src.PreparationTypeId,
        PreparationOrderId = src.PreparationOrderId,
        IsMenu = src.IsMenu ?? false,
        IsDeleted = src.DeletionDate != null,
        Prices = src.Prices?.Select(p => new PosProductPrice
        {
            PriceListId = p.PriceListId,
            MainPrice = p.MainPrice ?? p.Price,
            AddinPrice = p.AddinPrice,
            MenuItemPrice = p.MenuItemPrice
        }).ToList() ?? new()
    };

    public static PosFamily ToPos(AgoraFamily src) => new()
    {
        ExternalId = src.Id,
        Name = src.Name ?? string.Empty,
        ParentFamilyId = src.ParentFamilyId,
        Color = src.Color,
        Order = src.Order,
        IsDeleted = src.DeletionDate != null
    };

    public static PosPaymentMethod ToPos(AgoraPaymentMethod src) => new()
    {
        ExternalId = src.Id,
        Name = src.Name ?? string.Empty,
        GiveChange = src.GiveChange ?? false,
        IncludeInBalance = src.IncludeInBalance ?? false,
        IsValidForSale = src.IsValidForSale ?? true,
        IsDeleted = src.DeletionDate != null
    };

    public static PosVat ToPos(AgoraVat src) => new()
    {
        ExternalId = src.Id,
        Name = src.Name ?? string.Empty,
        VatRate = src.VatRate,
        SurchargeRate = src.SurchargeRate,
        Enabled = src.Enabled ?? true
    };

    public static PosSaleCenter ToPos(AgoraSaleCenter src) => new()
    {
        ExternalId = src.Id,
        Name = src.Name ?? string.Empty,
        PriceListId = src.PriceListId,
        VatIncluded = src.VatIncluded,
        Locations = src.SaleLocations?.Select(l => new PosSaleLocation { Name = l.Name ?? string.Empty }).ToList() ?? new(),
        IsDeleted = src.DeletionDate != null
    };

    public static PosUser ToPos(AgoraUser src) => new()
    {
        ExternalId = src.Id,
        Name = src.Name ?? string.Empty,
        Profile = src.Profile,
        IsDeleted = src.DeletionDate != null
    };

    public static PosCustomer ToPos(AgoraCustomer src) => new()
    {
        ExternalId = src.Id,
        FiscalName = src.FiscalName ?? string.Empty,
        BusinessName = src.BusinessName,
        Cif = src.Cif,
        CardNumber = src.CardNumber,
        Telephone = src.Telephone,
        Email = src.Email,
        IsDeleted = src.DeletionDate != null
    };

    public static PosInvoice ToPos(AgoraInvoice src) => new()
    {
        Serie = src.Serie ?? string.Empty,
        Number = src.Number,
        GlobalId = src.GlobalId,
        BusinessDay = src.BusinessDay,
        Date = src.Date,
        DocumentType = src.DocumentType,
        VatIncluded = src.VatIncluded,
        PosId = src.Pos?.Id,
        PosName = src.Pos?.Name,
        UserId = src.User?.Id,
        UserName = src.User?.Name,
        CustomerId = src.Customer?.Id,
        CustomerName = src.Customer?.Name,
        WorkplaceId = src.Workplace?.Id,
        WorkplaceName = src.Workplace?.Name,
        Totals = MapTotals(src.Totals),
        Lines = src.Lines?.Select(MapLine).ToList() ?? new(),
        Payments = src.Payments?.Select(MapPayment).ToList() ?? new(),
        ProcessedDate = string.IsNullOrEmpty(src.ProcessedDate) ? null : DateTime.Parse(src.ProcessedDate)
    };

    public static PosSalesOrder ToPos(AgoraSalesOrder src) => new()
    {
        Serie = src.Serie ?? string.Empty,
        Number = src.Number,
        GlobalId = src.GlobalId,
        BusinessDay = src.BusinessDay,
        Date = src.Date,
        Status = src.Status,
        OrderType = MapOrderType(src.Type),
        VatIncluded = src.VatIncluded,
        Guests = src.Guests,
        PosId = src.Pos?.Id,
        PosName = src.Pos?.Name,
        UserId = src.User?.Id,
        UserName = src.User?.Name,
        CustomerId = src.Customer?.Id,
        CustomerName = src.Customer?.Name,
        SaleCenterId = src.SaleCenter?.Id,
        SaleCenterName = src.SaleCenter?.Name,
        SaleLocationName = src.SaleCenter?.Location,
        Totals = MapTotals(src.Totals),
        Lines = src.Lines?.Select(MapLine).ToList() ?? new(),
        Payments = src.Payments?.Select(MapPayment).ToList() ?? new(),
        ProcessedDate = string.IsNullOrEmpty(src.ProcessedDate) ? null : DateTime.Parse(src.ProcessedDate)
    };

    public static PosCashCloseOut ToPos(AgoraPosCloseOut src) => new()
    {
        BusinessDay = src.BusinessDay,
        Date = src.Date,
        PosId = src.Pos?.Id,
        PosName = src.Pos?.Name,
        UserId = src.User?.Id,
        UserName = src.User?.Name,
        Totals = MapTotals(src.Totals)
    };

    // ── Helpers privados ──

    private static PosTotals MapTotals(AgoraTotals? src) => src == null ? new() : new()
    {
        GrossAmount = src.GrossAmount,
        NetAmount = src.NetAmount,
        VatAmount = src.VatAmount,
        SurchargeAmount = src.SurchargeAmount,
        Taxes = src.Taxes?.Select(t => new PosTaxDetail
        {
            VatRate = t.VatRate, SurchargeRate = t.SurchargeRate,
            GrossAmount = t.GrossAmount, NetAmount = t.NetAmount,
            VatAmount = t.VatAmount, SurchargeAmount = t.SurchargeAmount
        }).ToList() ?? new()
    };

    private static PosInvoiceLine MapLine(AgoraInvoiceLine src) => new()
    {
        ProductId = src.ProductId,
        ProductName = src.ProductName ?? string.Empty,
        SaleFormatId = src.SaleFormatId,
        SaleFormatName = src.SaleFormatName,
        Quantity = src.Quantity,
        Price = src.ProductPrice,
        TotalAmount = src.TotalAmount,
        VatId = src.VatId,
        VatRate = src.VatRate,
        DiscountRate = src.DiscountRate,
        CashDiscount = src.CashDiscount,
        FamilyId = src.FamilyId,
        FamilyName = src.FamilyName,
        LineType = src.Type,
        PriceListId = src.PriceListId
    };

    private static PosPayment MapPayment(AgoraPayment src) => new()
    {
        PaymentMethodId = src.PaymentMethodId,
        PaymentMethodName = src.PaymentMethodName ?? string.Empty,
        Amount = src.Amount,
        PaidAmount = src.PaidAmount,
        ChangeAmount = src.ChangeAmount,
        TipAmount = src.TipAmount,
        Date = src.Date,
        IsPrepayment = src.IsPrepayment,
        ExtraInformation = src.ExtraInformation
    };

    private static PosOrderType MapOrderType(string? type) => type?.ToLowerInvariant() switch
    {
        "table" => PosOrderType.Table,
        "takeaway" => PosOrderType.TakeAway,
        "delivery" => PosOrderType.Delivery,
        _ => PosOrderType.Standard
    };
}

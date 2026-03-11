using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KioskPos.Analytics.Analytics.Entities;
using KioskPos.Analytics.PosIntegration.Interfaces;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace KioskPos.Analytics.KioskManagement;

public class KioskManagementAppService : ApplicationService, IKioskManagementAppService
{
    private readonly IRepository<KioskRegistration, Guid> _kioskRepo;
    private readonly IPosIntegrationService _posService;

    public KioskManagementAppService(
        IRepository<KioskRegistration, Guid> kioskRepo,
        IPosIntegrationService posService)
    {
        _kioskRepo = kioskRepo;
        _posService = posService;
    }

    public async Task<List<KioskDto>> GetAllKiosksAsync()
    {
        var kiosks = await _kioskRepo.GetListAsync();
        return kiosks.OrderBy(k => k.SortOrder).Select(MapToDto).ToList();
    }

    public async Task<KioskDto> GetKioskAsync(Guid id)
    {
        var kiosk = await _kioskRepo.GetAsync(id);
        return MapToDto(kiosk);
    }

    public async Task<KioskDto> CreateKioskAsync(CreateUpdateKioskDto input)
    {
        var kiosk = new KioskRegistration
        {
            DisplayName = input.DisplayName,
            WorkplaceId = input.WorkplaceId,
            PosId = input.PosId,
            PosGroupId = input.PosGroupId,
            SaleCenterAquiId = input.SaleCenterAquiId,
            SaleCenterTakeAwayId = input.SaleCenterTakeAwayId,
            UserId = input.UserId,
            DefaultCustomerId = input.DefaultCustomerId,
            WarehouseId = input.WarehouseId,
            PriceListId = input.PriceListId,
            Color = input.Color,
            Icon = input.Icon,
            IsActive = input.IsActive,
            SortOrder = input.SortOrder
        };

        await _kioskRepo.InsertAsync(kiosk);
        return MapToDto(kiosk);
    }

    public async Task<KioskDto> UpdateKioskAsync(Guid id, CreateUpdateKioskDto input)
    {
        var kiosk = await _kioskRepo.GetAsync(id);
        kiosk.DisplayName = input.DisplayName;
        kiosk.WorkplaceId = input.WorkplaceId;
        kiosk.PosId = input.PosId;
        kiosk.PosGroupId = input.PosGroupId;
        kiosk.SaleCenterAquiId = input.SaleCenterAquiId;
        kiosk.SaleCenterTakeAwayId = input.SaleCenterTakeAwayId;
        kiosk.UserId = input.UserId;
        kiosk.DefaultCustomerId = input.DefaultCustomerId;
        kiosk.WarehouseId = input.WarehouseId;
        kiosk.PriceListId = input.PriceListId;
        kiosk.Color = input.Color;
        kiosk.Icon = input.Icon;
        kiosk.IsActive = input.IsActive;
        kiosk.SortOrder = input.SortOrder;

        await _kioskRepo.UpdateAsync(kiosk);
        return MapToDto(kiosk);
    }

    public async Task DeleteKioskAsync(Guid id)
    {
        await _kioskRepo.DeleteAsync(id);
    }

    public async Task<KioskConnectionTestDto> TestKioskConnectionAsync(Guid id)
    {
        var kiosk = await _kioskRepo.GetAsync(id);
        try
        {
            var connected = await _posService.TestConnectionAsync();
            var day = DateOnly.FromDateTime(DateTime.Today);
            var orders = await _posService.GetSalesOrdersAsync(day, includeProcessed: true);
            // Filtrar por workplace del kiosko
            var kioskOrders = orders.Where(o => o.WorkplaceId == kiosk.WorkplaceId).ToList();

            return new KioskConnectionTestDto
            {
                KioskId = kiosk.Id,
                KioskName = kiosk.DisplayName,
                Success = connected,
                Message = $"Conexión OK. {kioskOrders.Count} pedidos hoy para {kiosk.DisplayName}",
                OrderCountToday = kioskOrders.Count
            };
        }
        catch (Exception ex)
        {
            return new KioskConnectionTestDto
            {
                KioskId = kiosk.Id,
                KioskName = kiosk.DisplayName,
                Success = false,
                Message = ex.Message
            };
        }
    }

    private static KioskDto MapToDto(KioskRegistration k) => new()
    {
        Id = k.Id,
        DisplayName = k.DisplayName,
        WorkplaceId = k.WorkplaceId,
        PosId = k.PosId,
        PosGroupId = k.PosGroupId,
        SaleCenterAquiId = k.SaleCenterAquiId,
        SaleCenterTakeAwayId = k.SaleCenterTakeAwayId,
        UserId = k.UserId,
        DefaultCustomerId = k.DefaultCustomerId,
        WarehouseId = k.WarehouseId,
        PriceListId = k.PriceListId,
        Color = k.Color,
        Icon = k.Icon,
        IsActive = k.IsActive,
        SortOrder = k.SortOrder
    };
}

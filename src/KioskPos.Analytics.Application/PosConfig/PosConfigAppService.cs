using System.Linq;
using System.Threading.Tasks;
using KioskPos.Analytics.PosIntegration.Interfaces;
using Volo.Abp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using KioskPos.Analytics.Permissions;

namespace KioskPos.Analytics.PosConfig;

[Authorize(AnalyticsPermissions.PosConfig.Default)]
public class PosConfigAppService : ApplicationService, IPosConfigAppService
{
    private readonly IPosIntegrationService _posService;

    public PosConfigAppService(IPosIntegrationService posService)
    {
        _posService = posService;
    }

    public async Task<PosConnectionTestResultDto> TestConnectionAsync()
    {
        try
        {
            var connected = await _posService.TestConnectionAsync();
            if (!connected)
                return new PosConnectionTestResultDto { Success = false, Message = "No se pudo conectar con el POS" };

            var products = await _posService.GetProductsAsync();
            var families = await _posService.GetFamiliesAsync();

            return new PosConnectionTestResultDto
            {
                Success = true,
                Message = "Conexión exitosa con AgoraPOS",
                ProductCount = products.Count,
                FamilyCount = families.Count
            };
        }
        catch (System.Exception ex)
        {
            return new PosConnectionTestResultDto
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    public async Task<PosMasterDataSummaryDto> GetMasterDataSummaryAsync()
    {
        var productsTask = _posService.GetProductsAsync();
        var familiesTask = _posService.GetFamiliesAsync();
        var paymentMethodsTask = _posService.GetPaymentMethodsAsync();
        var saleCentersTask = _posService.GetSaleCentersAsync();
        var usersTask = _posService.GetUsersAsync();
        var vatsTask = _posService.GetVatsAsync();

        await System.Threading.Tasks.Task.WhenAll(
            productsTask, familiesTask, paymentMethodsTask,
            saleCentersTask, usersTask, vatsTask);

        return new PosMasterDataSummaryDto
        {
            ProductCount = productsTask.Result.Count,
            FamilyCount = familiesTask.Result.Count,
            PaymentMethodCount = paymentMethodsTask.Result.Count,
            SaleCenterCount = saleCentersTask.Result.Count,
            UserCount = usersTask.Result.Count,
            VatCount = vatsTask.Result.Count,
            Families = familiesTask.Result
                .Select(f => new PosFamilySummaryDto { Id = f.ExternalId, Name = f.Name, Order = f.Order })
                .OrderBy(f => f.Order)
                .ToList()
        };
    }
}

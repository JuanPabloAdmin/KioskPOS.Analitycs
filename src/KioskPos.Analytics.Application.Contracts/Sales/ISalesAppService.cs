using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace KioskPos.Analytics.Sales;

public interface ISalesAppService : IApplicationService
{
    Task<SalesListResultDto<SalesOrderDto>> GetSalesOrdersAsync(SalesFilterDto filter);
    Task<SalesListResultDto<InvoiceDto>> GetInvoicesAsync(SalesFilterDto filter);
    Task<SalesOrderDto> GetSalesOrderDetailAsync(string serie, int number, DateTime businessDay);
    Task<InvoiceDto> GetInvoiceDetailAsync(string serie, int number, DateTime businessDay);
}

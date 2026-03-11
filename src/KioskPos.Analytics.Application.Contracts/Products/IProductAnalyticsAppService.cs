using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace KioskPos.Analytics.Products;

public interface IProductAnalyticsAppService : IApplicationService
{
    Task<ProductAnalyticsSummaryDto> GetProductAnalyticsAsync(DateTime businessDay);
    Task<List<ProductPerformanceDto>> GetTopProductsAsync(DateTime businessDay, int count = 10);
    Task<List<FamilyPerformanceDto>> GetFamilyBreakdownAsync(DateTime businessDay);
    Task<ProductPerformanceDto> GetProductDetailAsync(int productId, DateTime businessDay);
}

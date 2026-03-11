using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace KioskPos.Analytics.PosConfig;

public interface IPosConfigAppService : IApplicationService
{
    Task<PosConnectionTestResultDto> TestConnectionAsync();
    Task<PosMasterDataSummaryDto> GetMasterDataSummaryAsync();
}

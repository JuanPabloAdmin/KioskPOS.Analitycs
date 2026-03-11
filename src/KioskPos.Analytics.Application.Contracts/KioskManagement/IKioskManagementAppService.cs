using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace KioskPos.Analytics.KioskManagement;

public interface IKioskManagementAppService : IApplicationService
{
    Task<List<KioskDto>> GetAllKiosksAsync();
    Task<KioskDto> GetKioskAsync(Guid id);
    Task<KioskDto> CreateKioskAsync(CreateUpdateKioskDto input);
    Task<KioskDto> UpdateKioskAsync(Guid id, CreateUpdateKioskDto input);
    Task DeleteKioskAsync(Guid id);
    Task<KioskConnectionTestDto> TestKioskConnectionAsync(Guid id);
}

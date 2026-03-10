using KioskPos.Analytics.Localization;
using Volo.Abp.AspNetCore.Components;

namespace KioskPos.Analytics.Blazor.Server;

public abstract class AnalyticsComponentBase : AbpComponentBase
{
    protected AnalyticsComponentBase()
    {
        LocalizationResource = typeof(AnalyticsResource);
    }
}

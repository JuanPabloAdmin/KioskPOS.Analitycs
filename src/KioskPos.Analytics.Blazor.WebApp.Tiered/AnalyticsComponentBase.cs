using KioskPos.Analytics.Localization;
using Volo.Abp.AspNetCore.Components;

namespace KioskPos.Analytics.Blazor.WebApp.Tiered;

public abstract class AnalyticsComponentBase : AbpComponentBase
{
    protected AnalyticsComponentBase()
    {
        LocalizationResource = typeof(AnalyticsResource);
    }
}

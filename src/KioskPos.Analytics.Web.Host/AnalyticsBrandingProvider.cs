using Microsoft.Extensions.Localization;
using KioskPos.Analytics.Localization;
using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace KioskPos.Analytics.Web;

[Dependency(ReplaceServices = true)]
public class AnalyticsBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<AnalyticsResource> _localizer;

    public AnalyticsBrandingProvider(IStringLocalizer<AnalyticsResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}

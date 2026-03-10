using KioskPos.Analytics.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace KioskPos.Analytics.Web.Pages;

public abstract class AnalyticsPageModel : AbpPageModel
{
    protected AnalyticsPageModel()
    {
        LocalizationResourceType = typeof(AnalyticsResource);
    }
}

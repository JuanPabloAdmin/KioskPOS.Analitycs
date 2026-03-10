using KioskPos.Analytics.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace KioskPos.Analytics.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class AnalyticsPageModel : AbpPageModel
{
    protected AnalyticsPageModel()
    {
        LocalizationResourceType = typeof(AnalyticsResource);
    }
}

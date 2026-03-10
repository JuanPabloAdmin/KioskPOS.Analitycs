using KioskPos.Analytics.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace KioskPos.Analytics.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class AnalyticsController : AbpControllerBase
{
    protected AnalyticsController()
    {
        LocalizationResource = typeof(AnalyticsResource);
    }
}

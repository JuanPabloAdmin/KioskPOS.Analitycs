using System;
using System.Collections.Generic;
using System.Text;
using KioskPos.Analytics.Localization;
using Volo.Abp.Application.Services;

namespace KioskPos.Analytics;

/* Inherit your application services from this class.
 */
public abstract class AnalyticsAppService : ApplicationService
{
    protected AnalyticsAppService()
    {
        LocalizationResource = typeof(AnalyticsResource);
    }
}

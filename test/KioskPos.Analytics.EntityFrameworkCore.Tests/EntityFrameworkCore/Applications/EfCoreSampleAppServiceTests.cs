using KioskPos.Analytics.Samples;
using Xunit;

namespace KioskPos.Analytics.EntityFrameworkCore.Applications;

[Collection(AnalyticsTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<AnalyticsEntityFrameworkCoreTestModule>
{

}

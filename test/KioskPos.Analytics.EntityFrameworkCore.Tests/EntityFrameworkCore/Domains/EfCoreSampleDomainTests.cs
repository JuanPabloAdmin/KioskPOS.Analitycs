using KioskPos.Analytics.Samples;
using Xunit;

namespace KioskPos.Analytics.EntityFrameworkCore.Domains;

[Collection(AnalyticsTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<AnalyticsEntityFrameworkCoreTestModule>
{

}

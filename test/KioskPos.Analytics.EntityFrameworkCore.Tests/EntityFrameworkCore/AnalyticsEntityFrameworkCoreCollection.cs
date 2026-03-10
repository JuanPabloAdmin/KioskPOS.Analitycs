using Xunit;

namespace KioskPos.Analytics.EntityFrameworkCore;

[CollectionDefinition(AnalyticsTestConsts.CollectionDefinitionName)]
public class AnalyticsEntityFrameworkCoreCollection : ICollectionFixture<AnalyticsEntityFrameworkCoreFixture>
{

}

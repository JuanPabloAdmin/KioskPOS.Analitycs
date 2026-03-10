using KioskPos.Analytics.MongoDB;
using KioskPos.Analytics.Samples;
using Xunit;

namespace KioskPos.Analytics.MongoDb.Applications;

[Collection(AnalyticsTestConsts.CollectionDefinitionName)]
public class MongoDBSampleAppServiceTests : SampleAppServiceTests<AnalyticsMongoDbTestModule>
{

}

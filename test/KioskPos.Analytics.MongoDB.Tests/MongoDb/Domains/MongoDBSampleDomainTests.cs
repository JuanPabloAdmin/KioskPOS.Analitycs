using KioskPos.Analytics.Samples;
using Xunit;

namespace KioskPos.Analytics.MongoDB.Domains;

[Collection(AnalyticsTestConsts.CollectionDefinitionName)]
public class MongoDBSampleDomainTests : SampleDomainTests<AnalyticsMongoDbTestModule>
{

}

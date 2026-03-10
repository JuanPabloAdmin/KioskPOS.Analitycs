using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace KioskPos.Analytics.Pages;

public class Index_Tests : AnalyticsWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}

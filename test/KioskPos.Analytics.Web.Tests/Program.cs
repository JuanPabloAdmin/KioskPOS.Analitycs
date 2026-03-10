using Microsoft.AspNetCore.Builder;
using KioskPos.Analytics;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();

builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("KioskPos.Analytics.Web.csproj");
await builder.RunAbpModuleAsync<AnalyticsWebTestModule>(applicationName: "KioskPos.Analytics.Web" );

public partial class Program
{
}

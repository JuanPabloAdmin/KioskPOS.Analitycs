using KioskPos.Analytics.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace KioskPos.Analytics.Permissions;

public class AnalyticsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var analyticsGroup = context.AddGroup(
            AnalyticsPermissions.GroupName,
            L("Permission:Analytics"));

        // Dashboard
        var dashboard = analyticsGroup.AddPermission(
            AnalyticsPermissions.Dashboard.Default,
            L("Permission:Dashboard"));
        dashboard.AddChild(
            AnalyticsPermissions.Dashboard.ViewRevenue,
            L("Permission:Dashboard.ViewRevenue"));
        dashboard.AddChild(
            AnalyticsPermissions.Dashboard.ViewProducts,
            L("Permission:Dashboard.ViewProducts"));
        dashboard.AddChild(
            AnalyticsPermissions.Dashboard.ExportData,
            L("Permission:Dashboard.ExportData"));

        // Sales
        var sales = analyticsGroup.AddPermission(
            AnalyticsPermissions.Sales.Default,
            L("Permission:Sales"));
        sales.AddChild(
            AnalyticsPermissions.Sales.ViewOrders,
            L("Permission:Sales.ViewOrders"));
        sales.AddChild(
            AnalyticsPermissions.Sales.ViewInvoices,
            L("Permission:Sales.ViewInvoices"));
        sales.AddChild(
            AnalyticsPermissions.Sales.ViewDetail,
            L("Permission:Sales.ViewDetail"));

        // Products
        var products = analyticsGroup.AddPermission(
            AnalyticsPermissions.Products.Default,
            L("Permission:Products"));
        products.AddChild(
            AnalyticsPermissions.Products.ViewAnalytics,
            L("Permission:Products.ViewAnalytics"));
        products.AddChild(
            AnalyticsPermissions.Products.ViewCosts,
            L("Permission:Products.ViewCosts"));

        // POS Config
        var posConfig = analyticsGroup.AddPermission(
            AnalyticsPermissions.PosConfig.Default,
            L("Permission:PosConfig"));
        posConfig.AddChild(
            AnalyticsPermissions.PosConfig.Manage,
            L("Permission:PosConfig.Manage"));
        posConfig.AddChild(
            AnalyticsPermissions.PosConfig.TestConnection,
            L("Permission:PosConfig.TestConnection"));
        posConfig.AddChild(
            AnalyticsPermissions.PosConfig.Sync,
            L("Permission:PosConfig.Sync"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AnalyticsResource>(name);
    }
}

namespace KioskPos.Analytics.Permissions;

public static class AnalyticsPermissions
{
    public const string GroupName = "Analytics";

    public static class Dashboard
    {
        public const string Default = GroupName + ".Dashboard";
        public const string ViewRevenue = Default + ".ViewRevenue";
        public const string ViewProducts = Default + ".ViewProducts";
        public const string ExportData = Default + ".ExportData";
    }

    public static class Sales
    {
        public const string Default = GroupName + ".Sales";
        public const string ViewOrders = Default + ".ViewOrders";
        public const string ViewInvoices = Default + ".ViewInvoices";
        public const string ViewDetail = Default + ".ViewDetail";
    }

    public static class Products
    {
        public const string Default = GroupName + ".Products";
        public const string ViewAnalytics = Default + ".ViewAnalytics";
        public const string ViewCosts = Default + ".ViewCosts";
    }

    public static class PosConfig
    {
        public const string Default = GroupName + ".PosConfig";
        public const string Manage = Default + ".Manage";
        public const string TestConnection = Default + ".TestConnection";
        public const string Sync = Default + ".Sync";
    }
}

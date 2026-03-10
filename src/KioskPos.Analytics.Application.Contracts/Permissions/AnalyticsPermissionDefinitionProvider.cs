using KioskPos.Analytics.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace KioskPos.Analytics.Permissions;

public class AnalyticsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AnalyticsPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(AnalyticsPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AnalyticsResource>(name);
    }
}

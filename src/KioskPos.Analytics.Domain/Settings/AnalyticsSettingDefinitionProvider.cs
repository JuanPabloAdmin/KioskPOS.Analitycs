using Volo.Abp.Settings;

namespace KioskPos.Analytics.Settings;

public class AnalyticsSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(AnalyticsSettings.MySetting1));
    }
}

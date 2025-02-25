#if (UNITY_ANDROID || UNITY_IOS) && UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class AirbridgeSettingsBuildProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        // Android
        AirbridgeSettingsWindow.UpdateAndroidManifest();
        AirbridgeSettingsWindow.UpdateAndroidAirbridgeSettings();
        
        // iOS
        AirbridgeSettingsWindow.UpdateiOSAppSetting();
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
#endif
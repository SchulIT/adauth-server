using ServerCore.Settings;

namespace ServerGUI.Services.Settings
{
    public abstract class FileSettingsBase
    {
		protected static string GetFilePath() => JsonSettings.GetPath();
    }
}

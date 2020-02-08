using AuthServer.Core.Settings;

namespace AuthServer.GUI.Settings
{
    public abstract class FileSettingsBase
    {
		protected static string GetFilePath() => JsonSettings.GetPath();
    }
}

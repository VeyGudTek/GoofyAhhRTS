using Source.Shared.Services;
using Unity.VisualScripting;

namespace Source.Shared.Repositories
{
    public class Settings
    {
        public float CameraSpeed { get; set; } = 300f;
        public Settings Clone() => (Settings)MemberwiseClone();
    }

    public class SettingsRepository
    {
        private Settings Settings { get; set; }
        private const string FileName = "Settings";

        public SettingsRepository()
        {
            Settings = DataAccessService.ReadData<Settings>(FileName);
        }

        public Settings GetSettings() => Settings.Clone();

        public void SaveSettings(Settings newSettings)
        {
            Settings = newSettings.Clone();
            DataAccessService.WriteData(FileName, Settings);
        }
    }
}


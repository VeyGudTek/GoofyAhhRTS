using Source.Shared.Services;

namespace Source.Shared.Repositories
{
    public class Settings
    {
        public float CameraSpeed { get; set; } = 300f;
        public Settings Clone() => (Settings)this.MemberwiseClone();
    }

    public class SettingsRepository
    {
        public Settings Settings { get; set; }
        private const string FileName = "Settings";

        public SettingsRepository()
        {
            Settings = DataAccessService.ReadData<Settings>(FileName);
        }

        public void SaveSettings()
        {
            DataAccessService.WriteData(FileName, Settings);
        }
    }
}


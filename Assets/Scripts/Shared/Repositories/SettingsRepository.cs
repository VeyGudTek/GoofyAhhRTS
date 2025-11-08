using Source.Shared.Services;
using UnityEngine;

namespace Source.Shared.Repositories
{
    public class Settings
    {
        public float CameraSpeed = 300f;
        public Settings Clone() => (Settings)MemberwiseClone();
    }

    public class SettingsRepository : MonoBehaviour
    {
        private Settings Settings { get; set; } = new();
        private const string FileName = "Settings";

        private void Awake()
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


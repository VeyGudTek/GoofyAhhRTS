using Source.Shared.Repositories;
using Source.Shared.Services;
using Source.Shared.Services.Interfaces;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.Start.Services
{
    public class StartService : MonoBehaviour, IMenuService
    {
        [InitializationRequired]
        [SerializeField]
        private SettingsMenuService SettingsMenuService;
        [InitializationRequired]
        [SerializeField]
        private StartMenuService StartMenuService;
        [InitializationRequired]
        [SerializeField]
        private SceneService SceneService;
        [InitializationRequired]
        [SerializeField]
        private SettingsRepository SettingsRepository;

        [SerializeField]
        private string NextSceneName = "TimGamePlay";

        public void Awake()
        {
            this.CheckInitializeRequired();
            StartMenuService.InjectDependencies(this);
            SettingsMenuService.InjectDependencies(SettingsRepository, this);
        }

        public void Play()
        {
            if (SceneService == null) return;

            SceneService.LoadScene(NextSceneName);
        }

        public void CloseSettings()
        {
            if (SettingsMenuService == null || StartMenuService == null) return;

            SettingsMenuService.gameObject.SetActive(false);
            StartMenuService.gameObject.SetActive(true);
        }

        public void OpenSettings()
        {
            if (SettingsMenuService == null || StartMenuService == null) return;

            SettingsMenuService.gameObject.SetActive(true);
            StartMenuService.gameObject.SetActive(false);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}


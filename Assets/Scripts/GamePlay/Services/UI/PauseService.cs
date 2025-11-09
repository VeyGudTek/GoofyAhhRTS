using Source.Shared.Services;
using Source.Shared.Services.Interfaces;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.GamePlay.Services.UI
{
    public class PauseService : MonoBehaviour, IMenuService
    {
        [InitializationRequired]
        private SettingsMenuService SettingsMenuService { get; set; }
        [InitializationRequired]
        private PauseMenuService PauseMenuService { get; set; }
        [InitializationRequired]
        private GamePlayService GamePlayService { get; set; }
        [InitializationRequired]
        private SceneService SceneService { get; set; }

        [SerializeField]
        private string QuitSceneName = "TimStart";

        public void InjectDependencies(SettingsMenuService settingsMenuService, PauseMenuService pauseMenuService, GamePlayService gamePlayService, SceneService sceneService)
        {
            SettingsMenuService = settingsMenuService;
            PauseMenuService = pauseMenuService;
            GamePlayService = gamePlayService;
            SceneService = sceneService;
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        public void OpenPauseMenu()
        {
            if (PauseMenuService == null) return;

            PauseMenuService.gameObject.SetActive(true);
        }

        public void ProcessCancel()
        {
            if (PauseMenuService == null) return;

            if (PauseMenuService.gameObject.activeSelf)
            {
                PauseMenuService.ProcessCancel();
            }
            if (SettingsMenuService.gameObject.activeSelf)
            {
                SettingsMenuService.ProcessCancel();
            }
        }

        public void Resume()
        {
            if (PauseMenuService == null || GamePlayService == null) return;

            PauseMenuService.gameObject.SetActive(false);
            GamePlayService.UnPauseGame();
        }

        public void OpenSettings()
        {
            if (PauseMenuService == null || SettingsMenuService == null) return;

            PauseMenuService.gameObject.SetActive(false);
            SettingsMenuService.gameObject.SetActive(true);
        }

        public void CloseSettings()
        {
            if (PauseMenuService == null || SettingsMenuService == null) return;

            PauseMenuService.gameObject.SetActive(true); 
            SettingsMenuService.gameObject.SetActive(false);
        }

        public void Quit()
        {
            if (SceneService != null)
            {
                SceneService.LoadScene(QuitSceneName);
            }
        }
    }
}

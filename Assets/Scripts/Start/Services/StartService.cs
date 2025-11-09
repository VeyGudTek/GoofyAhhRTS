using Source.Shared.Repositories;
using Source.Shared.Services;
using Source.Shared.Services.Interfaces;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.Start.Services
{
    public class StartService : MonoBehaviour, IMenuService, IInputProcessorService
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
        [InitializationRequired]
        [SerializeField]
        private InputService InputService;

        [SerializeField]
        private string NextSceneName = "TimGamePlay";

        public void Awake()
        {
            this.CheckInitializeRequired();
            StartMenuService.InjectDependencies(this);
            SettingsMenuService.InjectDependencies(SettingsRepository, this);
            InputService.InjectDependencies(this);
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

        public void PrimaryClickEvent(bool isShift) { }
        public void PrimaryHoldEvent() { }
        public void PrimaryReleaseEvent() { }
        public void SecondaryClickEvent() { }
        public void SecondaryHoldEvent() { }
        public void SecondaryReleaseEvent() { }
        public void FixedMoveEvent(Vector2 moveVector) { }
        public void CancelEvent() 
        { 
            if (SettingsMenuService != null && SettingsMenuService.gameObject.activeSelf)
            {
                SettingsMenuService.ProcessCancel();
            }
        }
    }
}


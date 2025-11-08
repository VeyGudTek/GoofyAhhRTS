using Source.Shared.Repositories;
using Source.Shared.Services.Interfaces;
using Source.Shared.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Shared.Services
{
    public class SettingsMenuService : MonoBehaviour
    {
        [InitializationRequired]
        [SerializeField]
        private GameObject DiscardConfirmationObject;
        [InitializationRequired]
        [SerializeField]
        private Slider CameraSpeedSlider;

        [InitializationRequired]
        private SettingsRepository SettingsRepo { get; set; }
        [InitializationRequired]
        private IMenuService MenuService { get; set; }
        private Settings CurrentSettings { get; set; } = new();

        public void InjectDependencies(SettingsRepository settingsRepo, IMenuService menuService)
        {
            SettingsRepo = settingsRepo;
            MenuService = menuService;
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        private void OnEnable()
        {
            if (SettingsRepo == null) return;
            CurrentSettings = SettingsRepo.GetSettings();

            if (CameraSpeedSlider != null)
                CameraSpeedSlider.value = CurrentSettings.CameraSpeed;
        }

        public void OnCameraSpeedChange()
        {
            CurrentSettings.CameraSpeed = CameraSpeedSlider.value;
        }

        public void OnDiscard()
        {
            if (DiscardConfirmationObject == null) return;

            DiscardConfirmationObject.SetActive(true);
        }

        public void OnDiscardCancel()
        {
            if (DiscardConfirmationObject == null) return;

            DiscardConfirmationObject.SetActive(false);
        }

        public void OnDiscardConfirm()
        {
            if (DiscardConfirmationObject == null || MenuService == null) return;

            DiscardConfirmationObject.SetActive(false);
            MenuService.CloseSettings();
        }

        public void OnSave()
        {
            if (MenuService == null) return;

            SettingsRepo.SaveSettings(CurrentSettings);
            MenuService.CloseSettings();
        }
    }
}

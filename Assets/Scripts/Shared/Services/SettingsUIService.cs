using Source.Shared.Repositories;
using Source.Shared.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Shared.Services
{
    public class SettingsUIService : MonoBehaviour
    {
        [InitializationRequired]
        [SerializeField]
        private GameObject DiscardConfirmationObject;
        [InitializationRequired]
        [SerializeField]
        private Slider CameraSpeedSlider;

        [InitializationRequired]
        private GameObject MenuObject { get; set; }
        [InitializationRequired]
        private SettingsRepository SettingsRepo { get; set; }
        private Settings CurrentSettings { get; set; } = new();

        public void InjectDependencies(GameObject menuObject, SettingsRepository settingsRepo)
        {
            MenuObject = menuObject;
            SettingsRepo = settingsRepo;
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
            if (DiscardConfirmationObject == null) return;

            DiscardConfirmationObject.SetActive(false);
            ReturnToMenu();
        }

        public void OnSave()
        {
            SettingsRepo.SaveSettings(CurrentSettings);
            ReturnToMenu();
        }

        private void ReturnToMenu()
        {
            if (MenuObject == null) return;

            gameObject.SetActive(false);
            MenuObject.SetActive(true);
        }
    }
}

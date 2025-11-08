using Source.Shared.Repositories;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.Start.Services
{
    public class SettingsService : MonoBehaviour
    {
        [InitializationRequired]
        [SerializeField]
        private GameObject DiscardConfirmationObject;

        [InitializationRequired]
        private GameObject MenuObject { get; set; }
        [InitializationRequired]
        private SettingsRepository SettingsRepo { get; set; }

        public void InjectDependencies(GameObject menuObject, SettingsRepository settingsRepo)
        {
            MenuObject = menuObject;
            SettingsRepo = settingsRepo;
        }

        private void Start()
        {
            this.CheckInitializeRequired();
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

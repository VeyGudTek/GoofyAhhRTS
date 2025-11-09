using Source.Shared.Utilities;
using UnityEngine;

namespace Source.GamePlay.Services.UI
{
    public class PauseMenuService : MonoBehaviour
    {
        [InitializationRequired]
        [SerializeField]
        private GameObject QuitConfirmationObject;
        [InitializationRequired]
        private PauseService PauseService;

        public void InjectDependencies(PauseService pauseService)
        {
            PauseService = pauseService;
        }

        void Start()
        {
            this.CheckInitializeRequired();
        }

        public void OnResume()
        {
            if (PauseService == null) return;

            PauseService.Resume();
        }

        public void ProcessEscape()
        {
            if (PauseService == null || QuitConfirmationObject == null) return;

            if (QuitConfirmationObject.gameObject.activeSelf)
            {
                OnQuitCancel();
            }
            else
            {
                PauseService.Resume();
            }
        }

        public void OnSettings()
        {
            if (PauseService == null) return;

            PauseService.OpenSettings();
        }

        public void OnQuit()
        {
            if (QuitConfirmationObject == null) return;

            QuitConfirmationObject.SetActive(true);
        }

        public void OnQuitCancel()
        {
            if (QuitConfirmationObject == null) return;

            QuitConfirmationObject.SetActive(false);
        }

        public void OnQuitConfirm()
        {
            if (PauseService == null) return;

            PauseService.Quit();
        }
    }
}

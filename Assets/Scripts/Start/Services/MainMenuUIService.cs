using Source.Shared.Services;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.Start.Services
{
    public class MainMenuUIService : MonoBehaviour
    {
        [InitializationRequired]
        private SceneService SceneService { get; set; }
        [InitializationRequired]
        private GameObject SettingsObject { get; set; }
        [SerializeField]
        private string PlaySceneName = "TimGamePlay";

        public void InjectDependencies(SceneService sceneService, GameObject settingsObject)
        {
            SceneService = sceneService;
            SettingsObject = settingsObject;
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        public void OnPlay()
        {
            if (SceneService == null) return;

            SceneService.LoadScene(PlaySceneName);
        }

        public void OnSettings()
        {
            if (SettingsObject == null) return;

            SettingsObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnQuit()
        {
            Application.Quit();
        }
    }
}

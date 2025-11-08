using Source.Shared.Services;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.Start.Services
{
    public class InjectorService : MonoBehaviour
    {
        [InitializationRequired]
        [SerializeField]
        private MainMenuService MainMenuService;
        [InitializationRequired]
        [SerializeField]
        private SettingsService SettingsService;
        [InitializationRequired]
        [SerializeField]
        private SceneService SceneService;

        private void Awake()
        {
            this.CheckInitializeRequired();
            InjectDependencies();
        }

        private void InjectDependencies()
        {
            MainMenuService.InjectDependencies(SceneService, SettingsService.gameObject);
            SettingsService.InjectDependencies(MainMenuService.gameObject);
        }
    }
}

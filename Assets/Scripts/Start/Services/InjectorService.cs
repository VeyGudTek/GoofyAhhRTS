using Source.Shared.Repositories;
using Source.Shared.Services;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.Start.Services
{
    public class InjectorService : MonoBehaviour
    {
        [InitializationRequired]
        [SerializeField]
        private MainMenuUIService MainMenuService;
        [InitializationRequired]
        [SerializeField]
        private SettingsUIService SettingsService;
        [InitializationRequired]
        [SerializeField]
        private SceneService SceneService;
        [InitializationRequired]
        [SerializeField]
        private SettingsRepository SettingsRepository;

        private void Awake()
        {
            this.CheckInitializeRequired();
            InjectDependencies();
        }

        private void InjectDependencies()
        {
            MainMenuService.InjectDependencies(SceneService, SettingsService.gameObject);
            SettingsService.InjectDependencies(MainMenuService.gameObject, SettingsRepository);
        }
    }
}

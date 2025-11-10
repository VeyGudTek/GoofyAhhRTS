using Source.Shared.Services;
using Source.Shared.Utilities;
using Source.GamePlay.Services.Unit;
using UnityEngine;
using Source.Shared.Repositories;
using Source.GamePlay.Services.UI;

namespace Source.GamePlay.Services
{
    public class InjectorService : MonoBehaviour
    {
        [SerializeField]
        [InitializationRequired]
        SelectionService SelectionService;
        [SerializeField]
        [InitializationRequired]
        private InputService InputService;
        [SerializeField]
        [InitializationRequired]
        private GamePlayService GamePlayService;
        [SerializeField]
        [InitializationRequired]
        private CameraService CameraService;
        [InitializationRequired]
        [SerializeField]
        private UnitManagerService UnitManagerService;
        [InitializationRequired]
        [SerializeField]
        private UnitDataService UnitDataService;
        [InitializationRequired]
        [SerializeField]
        private SettingsRepository SettingsRepository;
        [InitializationRequired]
        [SerializeField]
        private SettingsMenuService SettingsMenuService;
        [InitializationRequired]
        [SerializeField]
        private PauseMenuService PauseMenuService;
        [InitializationRequired]
        [SerializeField]
        private SceneService SceneService;
        [InitializationRequired]
        [SerializeField]
        private PauseService PauseService;

        private void Awake()
        {
            this.CheckInitializeRequired();
            InjectDependencies();
        }

        private void InjectDependencies()
        {
            PauseService.InjectDependencies(SettingsMenuService, PauseMenuService, GamePlayService, SceneService);
            PauseMenuService.InjectDependencies(PauseService);
            SettingsMenuService.InjectDependencies(SettingsRepository, PauseService);
            CameraService.InjectDependencies(SettingsRepository);
            SelectionService.InjectDependencies(CameraService);
            InputService.InjectDependencies(GamePlayService);
            UnitManagerService.InjectDependencies(UnitDataService, GamePlayService);
            GamePlayService.InjectDependencies(CameraService, UnitManagerService, SelectionService, PauseService, SceneService);
        }
    }
}

using Source.Shared.Services;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class InjectorService: MonoBehaviour
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

        private void Awake()
        {
            this.CheckInitializeRequired();
            InjectDependencies();
        }

        private void InjectDependencies()
        {
            SelectionService.InjectDependencies(CameraService);
            InputService.InjectDependencies(GamePlayService);
            GamePlayService.InjectDependencies(CameraService, UnitManagerService, SelectionService);
        }
    }
}

using Source.Shared.Services;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class InjectorService: MonoBehaviour
    {
        [InitializationRequired]
        [SerializeField]
        GameObject SelectionObject;
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
        private UnitManagerService UnitManagerService { get; set; }

        private void Awake()
        {
            UnitManagerService = new UnitManagerService();
            this.CheckInitializeRequired();
            InjectDependencies();
        }

        private void InjectDependencies()
        {
            SelectionService.InjectDependencies(CameraService, SelectionObject);
            InputService.InjectDependencies(GamePlayService);
            GamePlayService.InjectDependencies(CameraService, UnitManagerService, SelectionService);
        }
    }
}

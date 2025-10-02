using Source.Shared.Services;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class InjectorService: MonoBehaviour
    {
        private InputService InputService { get; set; }
        private GamePlayService GamePlayService { get; set; }
        private CameraService CameraService { get; set; }
        private UnitManagerService UnitManagerService { get; set; }

        private void Awake()
        {
            UnitManagerService = new UnitManagerService();

            InjectDependencies();
        }

        private void InjectDependencies()
        {
            InputService.InjectDependencies(GamePlayService);
            GamePlayService.InjectDependencies(CameraService, UnitManagerService);
        }
    }
}

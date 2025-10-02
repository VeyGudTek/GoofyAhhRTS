using Source.Shared.Services;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class InjectorService: MonoBehaviour
    {
        [SerializeField]
        private InputService InputService;
        [SerializeField]
        private GamePlayService GamePlayService;
        [SerializeField]
        private CameraService CameraService;
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

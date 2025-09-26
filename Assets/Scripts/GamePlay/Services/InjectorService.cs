using Source.GamePlay.Services.Interfaces;
using Source.Shared.Services.Interfaces;

namespace Source.GamePlay.Services
{
    public class InjectorService
    {
        private IInputService InputService;
        private IGamePlayService GamePlayService; 
        private ICameraService CameraService;
        private IUnitManagerService UnitManagerService;

        public InjectorService(IInputService inputService, ICameraService cameraService, IGamePlayService gamePlayService, IUnitManagerService unitManagerService)
        {
            InputService = inputService;
            CameraService = cameraService;
            GamePlayService = gamePlayService;
            UnitManagerService = unitManagerService;

            InjectDependencies();
        }

        private void InjectDependencies()
        {
            InputService.InjectDependencies(GamePlayService);
            GamePlayService.InjectDependencies(CameraService, UnitManagerService);
        }

        public void OnUpdate()
        {
            InputService.OnUpdate();
        }
    }
}

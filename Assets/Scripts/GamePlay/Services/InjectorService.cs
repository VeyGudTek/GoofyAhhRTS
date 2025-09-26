using Source.GamePlay.Services.Interfaces;
using Source.Shared.Services.Interfaces;

namespace Source.GamePlay.Services
{
    public class InjectorService
    {
        private IInputService InputService;
        private IGamePlayService GamePlayService; 
        private ICameraService CameraService;
        private IUnitManagerService UnitService;

        public InjectorService(IInputService inputService, ICameraService cameraService, IGamePlayService gamePlayService, IUnitManagerService unitService)
        {
            InputService = inputService;
            CameraService = cameraService;
            GamePlayService = gamePlayService;
            UnitService = unitService;

            InjectDependencies();
        }

        private void InjectDependencies()
        {
            InputService.InjectDependencies(GamePlayService);
            GamePlayService.InjectDependencies(CameraService, UnitService);
        }

        public void OnUpdate()
        {
            InputService.OnUpdate();
        }
    }
}

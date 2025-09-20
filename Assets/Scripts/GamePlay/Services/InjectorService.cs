using Source.GamePlay.Services.Interfaces;
using Source.Shared.Services;

namespace Source.GamePlay.Services
{
    public class InjectorService
    {
        private InputService InputService; //CHANGE TO INTERFACE
        private IGamePlayService GamePlayService; 
        private ICameraService CameraService;
        private IUnitService UnitService;

        public InjectorService(InputService inputService, ICameraService cameraService, IGamePlayService gamePlayService, IUnitService unitService)
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

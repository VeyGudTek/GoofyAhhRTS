using Source.GamePlay.Services.Interfaces;
using Source.GamePlay.Services.Units;
using Source.Shared.Services;

namespace Source.GamePlay.Services
{
    public class InjectorService
    {
        private InputService InputService; //CHANGE TO INTERFACE
        private IGamePlayService GamePlayService; 
        private ICameraService CameraService;
        private UnitService UnitService;

        public InjectorService(InputService inputService, ICameraService cameraService, IGamePlayService gamePlayService)
        {
            InputService = inputService;
            CameraService = cameraService;
            GamePlayService = gamePlayService;
            UnitService = new();

            InjectDependencies();
        }

        private void InjectDependencies()
        {
            InputService.InjectDependencies(GamePlayService);
            GamePlayService.InjectDependencies(CameraService, UnitService);
        }
    }
}

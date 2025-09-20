using Source.GamePlay.Services.Interfaces;
using Source.GamePlay.Services.Units;
using Source.Shared.Services;

namespace Source.GamePlay.Services
{
    public class InjectorService
    {
        private InputService InputService; //CHANGE TO INTERFACE
        private GamePlayService GamePlayService; //CHANGE TO INTERFACE
        private ICameraService CameraService; //CHANGE TO INTERFACE
        private UnitService UnitService;

        public InjectorService(InputService inputService, CameraService cameraService)
        {
            InputService = inputService;
            CameraService = cameraService;
            GamePlayService = new();
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

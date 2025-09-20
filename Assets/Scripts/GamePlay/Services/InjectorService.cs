using Source.GamePlay.Services.Units;
using Source.Shared.Services;

namespace Source.GamePlay.Services
{
    public class InjectorService
    {
        private InputService InputService; //CHANGE TO INTERFACE
        private GamePlayService GamePlayService; //CHANGE TO INTERFACE
        private CameraService CameraService; //CHANGE TO INTERFACE
        private UnitService UnitService;

        public void OnStart(InputService inputService, CameraService cameraService)
        {
            InputService = inputService;
            CameraService = cameraService;
            GamePlayService = new();
            UnitService = new();

            InjectDependencies();
        }

        private void InjectDependencies()
        {
            InputService.InjectDependencies(CameraService, GamePlayService);
            GamePlayService.InjectDependencies(InputService, UnitService);
        }
    }
}

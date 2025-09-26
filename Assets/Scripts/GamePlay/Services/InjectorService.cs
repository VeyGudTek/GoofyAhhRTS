using Source.GamePlay.Services.Interfaces;
using Source.Shared.Services.Interfaces;
using Source.Shared.HumbleObjects.Interfaces;
using Source.GamePlay.HumbleObjects.Interfaces;
using Source.Shared.Services;

namespace Source.GamePlay.Services
{
    public class InjectorService
    {
        private IInputService InputService;
        private IInputProcessorService GamePlayService; 
        private ICameraService CameraService;
        private IUnitManagerService UnitManagerService;

        public InjectorService(IInputHumbleObject inputHumbleObject, ICameraHumbleObject cameraHumbleObject)
        {
            UnitManagerService = new UnitManagerService();
            CameraService = new CameraService(cameraHumbleObject);
            GamePlayService = new GamePlayService(CameraService, UnitManagerService);
            InputService = new InputService(inputHumbleObject, GamePlayService);
        }

        public void OnUpdate()
        {
            InputService.OnUpdate();
        }
    }
}

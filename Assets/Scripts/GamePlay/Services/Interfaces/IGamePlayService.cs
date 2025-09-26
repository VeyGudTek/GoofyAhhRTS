using Source.Shared.Services.Interfaces;

namespace Source.GamePlay.Services.Interfaces
{
    public interface IGamePlayService : IInputProcessorService
    {
        void InjectDependencies(ICameraService cameraService, IUnitManagerService unitManagerService);
    }
}
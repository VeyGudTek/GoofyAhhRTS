using Source.GamePlay.Services.Units.Interfaces;
using Source.Shared.Services.Interfaces;

namespace Source.GamePlay.Services.Interfaces
{
    public interface IGamePlayService : IInputProcessorService
    {
        void InjectDependencies(ICameraService cameraService, IUnitService unitService);
    }
}
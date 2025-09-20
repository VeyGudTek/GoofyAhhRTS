using Source.GamePlay.Services.Interfaces;

namespace Source.Shared.Services.Interfaces
{
    public interface IInputService
    {
        void InjectDependencies(IGamePlayService gamePlayService);
        void OnUpdate();
    }
}
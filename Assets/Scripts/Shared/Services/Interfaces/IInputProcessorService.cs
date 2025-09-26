using UnityEngine;

namespace Source.Shared.Services.Interfaces
{
    public interface IInputProcessorService
    {
        void PrimaryClickEvent();
        void PrimaryHoldEvent();
        void PrimaryReleaseEvent();
        void SecondaryClickEvent();
        void SecondaryHoldEvent();
        void SecondaryReleaseEvent();
        void MoveEvent(Vector2 moveVector);
    }
}
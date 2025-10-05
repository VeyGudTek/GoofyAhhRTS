using UnityEngine;

namespace Source.Shared.Services.Interfaces
{
    public interface IInputProcessorService
    {
        void PrimaryClickEvent(bool isShift);
        void PrimaryHoldEvent(bool isShift);
        void PrimaryReleaseEvent();
        void SecondaryClickEvent();
        void SecondaryHoldEvent();
        void SecondaryReleaseEvent();
        void MoveEvent(Vector2 moveVector);
    }
}
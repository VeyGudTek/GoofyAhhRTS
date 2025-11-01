using UnityEngine;

namespace Source.Shared.Services.Interfaces
{
    public interface IInputProcessorService
    {
        void PrimaryClickEvent(bool isShift);
        void PrimaryHoldEvent();
        void PrimaryReleaseEvent();
        void SecondaryClickEvent();
        void SecondaryHoldEvent();
        void SecondaryReleaseEvent();
        void FixedMoveEvent(Vector2 moveVector);
    }
}
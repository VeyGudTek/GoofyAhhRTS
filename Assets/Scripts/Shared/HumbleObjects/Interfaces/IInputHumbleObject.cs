using UnityEngine;

namespace Source.Shared.Controllers.Interfaces
{
    public interface IInputHumbleObject
    {
        bool PrimaryClicked();
        bool PrimaryHold();
        bool PrimaryReleased();
        bool SecondaryClicked();
        bool SecondaryHold();
        bool SecondaryReleased();
        Vector2 GetMove();
    }
}
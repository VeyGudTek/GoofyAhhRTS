using UnityEngine;

namespace Source.Shared.Controllers
{
    public interface IInputController
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
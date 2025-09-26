using Source.Shared.Utilities;
using Source.Shared.HumbleObjects.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Shared.Controllers
{
    public class InputHumbleObject : MonoBehaviour, IInputHumbleObject
    {
        [InitializationRequired]
        private InputAction Primary;
        [InitializationRequired]
        private InputAction Secondary;
        [InitializationRequired]
        private InputAction Move;

        private void Awake()
        {
            Primary = InputSystem.actions.FindAction("Attack");
            Secondary = InputSystem.actions.FindAction("RightClick");
            Move = InputSystem.actions.FindAction("Move");
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        public bool PrimaryClicked()
        {
            if (Primary != null)
            {
                return Primary.WasPerformedThisFrame();
            }
            return false;
        }
        public bool PrimaryHold()
        {
            if (Primary != null)
            {
                return Primary.IsInProgress();
            }
            return false;
        }
        public bool PrimaryReleased()
        {
            if (Primary != null)
            {
                return Primary.WasReleasedThisFrame();
            }
            return false;
        }
        public bool SecondaryClicked()
        {
            if (Secondary != null)
            {
                return Secondary.WasPerformedThisFrame();
            }
            return false;
        }
        public bool SecondaryHold()
        {
            if (Secondary != null)
            {
                return Secondary.IsInProgress();
            }
            return false;
        }
        public bool SecondaryReleased()
        {
            if (Secondary != null)
            {
                return Secondary.WasReleasedThisFrame();
            }
            return false;
        }
        public Vector2 GetMove()
        {
            if (Move != null)
            {
                return Move.ReadValue<Vector2>();
            }
            return Vector2.zero;
        }
    }
}
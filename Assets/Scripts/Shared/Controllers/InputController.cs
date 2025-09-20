using Source.Shared.Services;
using Source.Shared.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Shared.Controllers
{
    public class InputController : MonoBehaviour, IInputController
    {
        [InitializationRequired]
        private InputAction Primary;
        [InitializationRequired]
        private InputAction Secondary;
        [InitializationRequired]
        private InputAction Move;
        [InitializationRequired]
        public InputService InputService { get; private set; }

        private void Awake()
        {
            Primary = InputSystem.actions.FindAction("Attack");
            Secondary = InputSystem.actions.FindAction("RightClick");
            Move = InputSystem.actions.FindAction("Move");
            InputService = new InputService(this);
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        private void Update()
        {
            InputService.OnUpdate();
        }

        public bool PrimaryClicked()
        {
            if (Primary != null)
            {
                Primary.WasPressedThisFrame();
            }
            return false;
        }
        public bool PrimaryHold()
        {
            if (Primary != null)
            {
                Primary.IsPressed();
            }
            return false;
        }
        public bool PrimaryReleased()
        {
            if (Primary != null)
            {
                Primary.WasReleasedThisFrame();
            }
            return false;
        }
        public bool SecondaryClicked()
        {
            if (Secondary != null)
            {
                Secondary.WasPressedThisFrame();
            }
            return false;
        }
        public bool SecondaryHold()
        {
            if (Secondary != null)
            {
                Secondary.WasPressedThisFrame();
            }
            return false;
        }
        public bool SecondaryReleased()
        {
            if (Secondary != null)
            {
                Secondary.WasReleasedThisFrame();
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
using UnityEngine;
using UnityEngine.InputSystem;
using Source.Shared.Utilities;

namespace Source.Shared
{
    public delegate void ButtonCallback();
    public delegate void AxisCallback(float x, float y);
    public class InitializeInputCallbackDTO
    {
        public ButtonCallback PrimaryClickEvent = null;
        public ButtonCallback PrimaryHoldEvent = null;
        public ButtonCallback PrimaryReleaseEvent = null;
        public ButtonCallback SecondaryClickEvent = null;
        public ButtonCallback SecondaryHoldEvent = null;
        public ButtonCallback SecondaryReleaseEvent = null;
        public AxisCallback MoveEvent = null;
    }

    public class InputManager : InitializationRequiredMonoBehavior<InitializeInputCallbackDTO>
    {
        private InputAction Primary;
        private ButtonCallback PrimaryClickEvent = null;
        private ButtonCallback PrimaryHoldEvent = null;
        private ButtonCallback PrimaryReleaseEvent = null;

        private InputAction Secondary;
        private ButtonCallback SecondaryClickEvent = null;
        private ButtonCallback SecondaryHoldEvent = null;
        private ButtonCallback SecondaryReleaseEvent = null;

        private InputAction Move;
        private AxisCallback MoveEvent = null;

        override public void Initialize(InitializeInputCallbackDTO callbacks)
        {
            Initialized = true;
            PrimaryClickEvent = callbacks.PrimaryClickEvent;
            PrimaryHoldEvent = callbacks.PrimaryHoldEvent;
            PrimaryReleaseEvent = callbacks.PrimaryReleaseEvent;
            SecondaryClickEvent = callbacks.SecondaryClickEvent;
            SecondaryHoldEvent = callbacks.SecondaryHoldEvent;
            SecondaryReleaseEvent = callbacks.SecondaryReleaseEvent;
            MoveEvent = callbacks.MoveEvent;
        }

        void Awake()
        {
            Primary = InputSystem.actions.FindAction("Attack");
            Secondary = InputSystem.actions.FindAction("RightClick");
            Move = InputSystem.actions.FindAction("Move");
        }

        private void Start()
        {
            CheckInitialized();
        }


        void Update()
        {
            UpdatePrimary();
            UpdateSecondary();
            UpdateMovement();
        }

        void UpdatePrimary()
        {
            if (Primary.WasCompletedThisFrame())
            {
                PrimaryClickEvent?.Invoke();
            }
            if (Primary.inProgress)
            {
                PrimaryHoldEvent?.Invoke();
            }
            if (Primary.WasPressedThisFrame())
            {
                PrimaryReleaseEvent?.Invoke();
            }
        }

        void UpdateSecondary()
        {
            if (Secondary.WasCompletedThisFrame())
            {
                SecondaryClickEvent?.Invoke();
            }
            if (Secondary.inProgress)
            {
                SecondaryHoldEvent?.Invoke();
            }
            if (Secondary.WasPressedThisFrame())
            {
                SecondaryReleaseEvent?.Invoke();
            }
        }

        void UpdateMovement()
        {
            Vector2 moveVector = Move.ReadValue<Vector2>();
            MoveEvent?.Invoke(moveVector.x, moveVector.y);
        }
    }
}

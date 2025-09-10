using UnityEngine;
using UnityEngine.InputSystem;
using Source.Shared.Utilities;
using System;

namespace Source.Shared
{
    public class InitializeInputCallbackDTO
    {
        public Action PrimaryClickEvent = null;
        public Action PrimaryHoldEvent = null;
        public Action PrimaryReleaseEvent = null;
        public Action SecondaryClickEvent = null;
        public Action SecondaryHoldEvent = null;
        public Action SecondaryReleaseEvent = null;
        public Action<Vector2> MoveEvent = null;
    }

    public class InputManager : MonoBehaviour
    {
        [InitializationRequired]
        private InputAction Primary;
        [InitializationRequired]
        private Action PrimaryClickEvent = null;
        [InitializationRequired]
        private Action PrimaryHoldEvent = null;
        [InitializationRequired]
        private Action PrimaryReleaseEvent = null;

        [InitializationRequired]
        private InputAction Secondary;
        [InitializationRequired]
        private Action SecondaryClickEvent = null;
        [InitializationRequired]
        private Action SecondaryHoldEvent = null;
        [InitializationRequired]
        private Action SecondaryReleaseEvent = null;

        private InputAction Move;
        private Action<Vector2> MoveEvent = null;

        public void Initialize(InitializeInputCallbackDTO callbacks)
        {
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
            this.CheckInitializeRequired();
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
            MoveEvent?.Invoke(moveVector);
        }
    }
}

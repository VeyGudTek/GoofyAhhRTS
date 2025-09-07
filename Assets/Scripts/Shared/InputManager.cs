using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace Source.Shared
{
    public delegate void CallbackFunction();
    public class InitializeCallbackDTO
    {
        public CallbackFunction PrimaryClickEvent = null;
        public CallbackFunction PrimaryHoldEvent = null;
        public CallbackFunction PrimaryReleaseEvent = null;
        public CallbackFunction SecondaryClickEvent = null;
        public CallbackFunction SecondaryHoldEvent = null;
        public CallbackFunction SecondaryReleaseEvent = null;
    }

    public class InputManager : MonoBehaviour
    {
        private bool Initialized = false;

        private InputAction Primary;
        private CallbackFunction PrimaryClickEvent = null;
        private CallbackFunction PrimaryHoldEvent = null;
        private CallbackFunction PrimaryReleaseEvent = null;

        private InputAction Secondary;
        private CallbackFunction SecondaryClickEvent = null;
        private CallbackFunction SecondaryHoldEvent = null;
        private CallbackFunction SecondaryReleaseEvent = null;

        public void InitializeCallbacks(InitializeCallbackDTO callBacks)
        {
            Initialized = true;
            PrimaryClickEvent = callBacks.PrimaryClickEvent;
            PrimaryHoldEvent = callBacks.PrimaryHoldEvent;
            PrimaryReleaseEvent = callBacks.PrimaryReleaseEvent;
            SecondaryClickEvent = callBacks.SecondaryClickEvent;
            SecondaryHoldEvent = callBacks.SecondaryHoldEvent;
            SecondaryReleaseEvent = callBacks.SecondaryReleaseEvent;
        }

        void Awake()
        {
            Primary = InputSystem.actions.FindAction("Attack");
            Secondary = InputSystem.actions.FindAction("RightClick");
        }

        private void Start()
        {
            if (!Initialized)
            {
                throw new Exception("Callbacks not Initialized.");
            }
        }


        void Update()
        {
            UpdatePrimary();
            UpdateSecondary();
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
    }
}

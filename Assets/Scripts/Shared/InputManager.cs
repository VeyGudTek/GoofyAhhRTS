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
        public CallbackFunction PrimaryOffEvent = null;
        public CallbackFunction SecondaryClickEvent = null;
        public CallbackFunction SecondaryHoldEvent = null;
        public CallbackFunction SecondaryOffEvent = null;
    }

    public class InputManager : MonoBehaviour
    {
        private bool Initialized = false;

        private InputAction Primary;
        private CallbackFunction PrimaryClickEvent = null;
        private CallbackFunction PrimaryHoldEvent = null;
        private CallbackFunction PrimaryOffEvent = null;

        private InputAction Secondary;
        private CallbackFunction SecondaryClickEvent = null;
        private CallbackFunction SecondaryHoldEvent = null;
        private CallbackFunction SecondaryOffEvent = null;

        public void InitializeCallbacks(InitializeCallbackDTO callBacks)
        {
            Initialized = true;
            PrimaryClickEvent = callBacks.PrimaryClickEvent;
            PrimaryHoldEvent = callBacks.PrimaryHoldEvent;
            PrimaryOffEvent = callBacks.PrimaryOffEvent;
            SecondaryClickEvent = callBacks.SecondaryClickEvent;
            SecondaryHoldEvent = callBacks.SecondaryHoldEvent;
            SecondaryOffEvent = callBacks.SecondaryOffEvent;
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
                PrimaryOffEvent?.Invoke();
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
                SecondaryOffEvent?.Invoke();
            }
        }
    }
}

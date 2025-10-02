using Source.Shared.Services.Interfaces;
using Source.Shared.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Shared.Services
{
    public class InputService: MonoBehaviour
    {
        [InitializationRequired]
        private IInputProcessorService InputProcessorService { get; set; }
        [InitializationRequired]
        private InputAction Primary { get; set; }
        [InitializationRequired]
        private InputAction Secondary { get; set; }
        [InitializationRequired]
        private InputAction Move { get; set; }

        public void InjectDependencies(IInputProcessorService inputProcessorService)
        {
            InputProcessorService = inputProcessorService;
        }

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

        private void Update()
        {
            if (InputProcessorService == null) return;

            UpdatePrimary();
            UpdateSecondary();
            UpdateMovement();
        }

        void UpdatePrimary()
        {
            if (Primary == null)
            {
                return;
            }
            if (Primary.WasPerformedThisFrame())
            {
                InputProcessorService.PrimaryReleaseEvent();
            }
            if (Primary.IsInProgress())
            {
                InputProcessorService.PrimaryHoldEvent();
            }
            if (Primary.WasReleasedThisFrame())
            {
                InputProcessorService.PrimaryClickEvent();
            }
        }

        void UpdateSecondary()
        {
            if ( Secondary == null)
            {
                return;
            }
            if (Secondary.WasPerformedThisFrame())
            {
                InputProcessorService.SecondaryReleaseEvent();
            }
            if (Secondary.IsInProgress())
            {
                InputProcessorService.SecondaryHoldEvent();
            }
            if (Secondary.WasReleasedThisFrame())
            {
                InputProcessorService.SecondaryClickEvent();
            }
        }

        void UpdateMovement()
        {
            if (Move == null)
            {
                return;
            }
            Vector2 result = Move.ReadValue<Vector2>();
            if (result != Vector2.zero)
            {
                InputProcessorService.MoveEvent(result);
            }
        }
    }
}

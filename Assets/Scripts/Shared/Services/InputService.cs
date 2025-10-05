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
        [InitializationRequired]
        private InputAction Sprint { get; set; }

        public void InjectDependencies(IInputProcessorService inputProcessorService)
        {
            InputProcessorService = inputProcessorService;
        }

        private void Awake()
        {
            Primary = InputSystem.actions.FindAction("Attack");
            Secondary = InputSystem.actions.FindAction("RightClick");
            Move = InputSystem.actions.FindAction("Move");
            Sprint = InputSystem.actions.FindAction("Sprint");
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
            if (Sprint == null || Primary == null) return;

            if (Primary.WasPerformedThisFrame())
            {
                InputProcessorService.PrimaryClickEvent(Sprint.IsInProgress());
            }
            if (Primary.IsInProgress())
            {
                InputProcessorService.PrimaryHoldEvent(Sprint.IsInProgress());
            }
            if (Primary.WasReleasedThisFrame())
            {
                InputProcessorService.PrimaryReleaseEvent();
            }
        }

        void UpdateSecondary()
        {
            if ( Secondary == null)
                return;

            if (Secondary.WasPerformedThisFrame())
            {
                InputProcessorService.SecondaryClickEvent();
            }
            if (Secondary.IsInProgress())
            {
                InputProcessorService.SecondaryHoldEvent();
            }
            if (Secondary.WasReleasedThisFrame())
            {
                InputProcessorService.SecondaryReleaseEvent();
            }
        }

        void UpdateMovement()
        {
            if (Move == null) return;
            
            Vector2 result = Move.ReadValue<Vector2>();
            if (result != Vector2.zero)
            {
                InputProcessorService.MoveEvent(result);
            }
        }
    }
}

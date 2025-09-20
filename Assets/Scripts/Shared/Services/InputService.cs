using Source.GamePlay.Services.Interfaces;
using Source.Shared.Controllers;
using Source.Shared.Services.Interfaces;
using UnityEngine;

namespace Source.Shared.Services
{
    public class InputService
    {
        private IInputProcessorService InputProcessorService;
        private IInputController InputController;

        public InputService(IInputController inputController)
        {
            InputController = inputController;
        }

        public void InjectDependencies(IGamePlayService gamePlayService)
        {
            InputProcessorService = gamePlayService;
        }

        public void OnUpdate()
        {
            UpdatePrimary();
            UpdateSecondary();
            UpdateMovement();
        }

        void UpdatePrimary()
        {
            if (InputController.PrimaryClicked())
            {
                InputProcessorService.PrimaryReleaseEvent();
            }
            if (InputController.PrimaryClicked())
            {
                InputProcessorService.PrimaryHoldEvent();
            }
            if (InputController.PrimaryClicked())
            {
                InputProcessorService.PrimaryClickEvent();
            }
        }

        void UpdateSecondary()
        {
            if (InputController.PrimaryClicked())
            {
                InputProcessorService.SecondaryReleaseEvent();
            }
            if (InputController.PrimaryClicked())
            {
                InputProcessorService.SecondaryHoldEvent();
            }
            if (InputController.PrimaryClicked())
            {
                InputProcessorService.SecondaryClickEvent();
            }
        }

        void UpdateMovement()
        {
            Vector2 result = InputController.GetMove();
            if (result != Vector2.zero)
            {
                InputProcessorService.MoveEvent(result);
            }
        }
    }
}

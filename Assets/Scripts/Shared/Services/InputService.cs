using Source.GamePlay.Services;
using Source.Shared.Controllers;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.Shared.Services
{
    public class InputService
    {
        [InitializationRequired]
        private GamePlayService InputProcessorService; //CHANGE THIS TO GENERIC INTERFACE

        private IInputController InputController;

        public InputService(IInputController inputController)
        {
            InputController = inputController;
        }

        public void InjectDependencies(GamePlayService gamePlayService)
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
            if (InputController.GetMove() != Vector2.zero)
            {
                InputProcessorService.MoveEvent();
            }
        }
    }
}

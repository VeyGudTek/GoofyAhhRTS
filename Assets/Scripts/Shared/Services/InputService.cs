using Source.GamePlay.Services.Interfaces;
using Source.Shared.HumbleObjects.Interfaces;
using Source.Shared.Services.Interfaces;
using UnityEngine;

namespace Source.Shared.Services
{
    public class InputService: IInputService
    {
        private IInputProcessorService InputProcessorService;
        private IInputHumbleObject InputController;

        public InputService(IInputHumbleObject inputController)
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
            if (InputController.PrimaryReleased())
            {
                InputProcessorService.PrimaryReleaseEvent();
            }
            if (InputController.PrimaryHold())
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
            if (InputController.SecondaryReleased())
            {
                InputProcessorService.SecondaryReleaseEvent();
            }
            if (InputController.SecondaryHold())
            {
                InputProcessorService.SecondaryHoldEvent();
            }
            if (InputController.SecondaryClicked())
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

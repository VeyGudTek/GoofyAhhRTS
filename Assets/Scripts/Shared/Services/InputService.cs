using Source.GamePlay.Services.Interfaces;
using Source.Shared.HumbleObjects.Interfaces;
using Source.Shared.Services.Interfaces;
using UnityEngine;

namespace Source.Shared.Services
{
    public class InputService: IInputService
    {
        private IInputProcessorService InputProcessorService;
        private IInputHumbleObject InputHumbleObject;

        public InputService(IInputHumbleObject inputHumbleObject)
        {
            InputHumbleObject = inputHumbleObject;
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
            if (InputHumbleObject.PrimaryReleased())
            {
                InputProcessorService.PrimaryReleaseEvent();
            }
            if (InputHumbleObject.PrimaryHold())
            {
                InputProcessorService.PrimaryHoldEvent();
            }
            if (InputHumbleObject.PrimaryClicked())
            {
                InputProcessorService.PrimaryClickEvent();
            }
        }

        void UpdateSecondary()
        {
            if (InputHumbleObject.SecondaryReleased())
            {
                InputProcessorService.SecondaryReleaseEvent();
            }
            if (InputHumbleObject.SecondaryHold())
            {
                InputProcessorService.SecondaryHoldEvent();
            }
            if (InputHumbleObject.SecondaryClicked())
            {
                InputProcessorService.SecondaryClickEvent();
            }
        }

        void UpdateMovement()
        {
            Vector2 result = InputHumbleObject.GetMove();
            if (result != Vector2.zero)
            {
                InputProcessorService.MoveEvent(result);
            }
        }
    }
}

using Source.Shared.Services;
using Source.Shared.Utilities;
using System;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class GamePlayService
    {
        private InputService InputService;

        public void InjectDependencies(InputService inputService)
        {
            InputService = inputService;
        }

        public void PrimaryClickEvent()
        {
            ContactDto contact = InputService?.GetMouseWorldPoint();
            if (contact != null)
            {
                Debug.Log($"Hit GameObject: {contact.HitGameObject} | WorldPoint: {contact.Point} | GameObject: {contact.GameObject?.name}");
            }
        }

        public Action PrimaryHoldEvent = () => { };
        public Action PrimaryReleaseEvent = () => { };
        public Action SecondaryClickEvent = () => { };
        public Action SecondaryHoldEvent = () => { };
        public Action SecondaryReleaseEvent = () => { };
        public Action MoveEvent = () => { };
    }
}

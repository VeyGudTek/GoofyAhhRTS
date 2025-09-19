using Source.GamePlay.Services.Units;
using Source.Shared.Services;
using System;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class GamePlayService
    {
        private InputService InputService;
        private UnitService UnitService;

        public void InjectDependencies(InputService inputService, UnitService unitService)
        {
            InputService = inputService;
            UnitService = unitService;
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

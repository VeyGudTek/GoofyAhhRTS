using Source.GamePlay.HumbleObjects.Interfaces;
using System;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class UnitService
    {
        private IUnitHumbleObject UnitHumbleObject;

        private float Health;
        private float? Speed;
        public Guid PlayerId { get; private set; }
        public bool Selected { get; private set; } = false;

        public UnitService(Guid playerId, float health, float? speed, IUnitHumbleObject unitHumbleObject)
        {
            PlayerId = playerId;
            Health = health;
            UnitHumbleObject = unitHumbleObject;
            UnitHumbleObject.SetSpeed(speed);
        }

        public void MoveUnit(Vector3 destination)
        {
            UnitHumbleObject.MoveUnit(destination);
        }

        public Vector3 GetPosition()
        {
            return UnitHumbleObject.GetPosition();
        }

        public void Select()
        {
            Selected = true;
        }

        public void Deselect()
        {
            Selected = false;
        }
    }
}

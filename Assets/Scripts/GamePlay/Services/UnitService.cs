using Source.GamePlay.HumbleObjects.Interfaces;
using System;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class UnitService
    {
        private IUnitHumbleObject UnitController;

        private float Health;
        private float? Speed;
        public Guid PlayerId { get; private set; }
        public bool Selected { get; private set; } = false;

        public UnitService(Guid playerId, float health, float? speed, IUnitHumbleObject unitController)
        {
            PlayerId = playerId;
            Health = health;
            UnitController = unitController;
            UnitController.SetSpeed(speed);
        }

        public void MoveUnit(Vector3 destination)
        {
            UnitController.MoveUnit(destination);
        }

        public Vector3 GetPosition()
        {
            return UnitController.GetPosition();
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

using Source.GamePlay.Controllers.Interfaces;
using System;
using UnityEngine;

namespace Source.GamePlay.Services.Units
{
    public class Unit
    {
        private IUnitHumbleObject UnitController;

        private float Health;
        private float? Speed;
        public Guid PlayerId { get; private set; }
        public bool Selected { get; private set; } = false;

        public Unit(Guid playerId, float health, float? speed, IUnitHumbleObject unitController)
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

using Source.Shared.Utilities;
using System;
using UnityEngine;

namespace Source.GamePlay.Services.Units
{
    public class Unit
    {
        private Action<Vector3> MoveUnitCallback;
        private Func<Vector3> GetPositionCallback;

        private float Health;
        private float? Speed;
        public Guid PlayerId { get; private set; }
        public bool Selected { get; private set; } = false;

        public Unit(Guid playerId, float health, float? speed, Action<Vector3> moveUnit, Func<Vector3> getPositionCallback, Action<float?> setSpeed)
        {
            PlayerId = playerId;
            Health = health;
            MoveUnitCallback = moveUnit;
            GetPositionCallback = getPositionCallback;
            setSpeed(speed);
        }

        public void MoveUnit(Vector3 destination)
        {
            MoveUnitCallback(destination);
        }

        public Vector3 GetPosition()
        {
            return GetPositionCallback();
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

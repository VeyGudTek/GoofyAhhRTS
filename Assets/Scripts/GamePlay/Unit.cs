using Source.Shared.Utilities;
using System;
using UnityEngine;

namespace Source.GamePlay
{
    public class Unit : MonoBehaviour
    {
        [SerializeField]
        private UnitMovement UnitMovement;

        [InitializationRequired]
        private float? Health = null;
        [InitializationRequired]
        public Guid? PlayerId { get; private set; } = null;
        public bool Selected { get; private set; } = false;

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        public void Initialize(Guid playerId, float health, float speed)
        {
            PlayerId = playerId;
            Health = health;
            if (UnitMovement != null)
            {
                UnitMovement.Initialize(speed);
            }
        }

        public void MoveUnit(Vector3 destination)
        {
            if (UnitMovement == null)
            {
                UnitMovement.MoveUnit(destination);
            }
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

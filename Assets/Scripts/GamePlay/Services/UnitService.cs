using Source.Shared.Utilities;
using System;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class UnitService: MonoBehaviour
    {
        [SerializeField]
        [InitializationRequired]
        UnitMovementService UnitMovementService { get; set; }

        private float Health { get; set; }
        private float? Speed { get; set; }
        public Guid PlayerId { get; private set; }
        public bool Selected { get; set; } = false;

        public void MoveUnit(Vector3 destination)
        {
            UnitMovementService.MoveUnit(destination);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}

using Source.Shared.Utilities;
using System;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class UnitService : MonoBehaviour
    {
        [SerializeField]
        [InitializationRequired]
        private UnitMovementService UnitMovementService;

        [SerializeField]
        [InitializationRequired]
        private GameObject SelectionIndicator;

        private float Health { get; set; }
        private float? Speed { get; set; }
        public Guid PlayerId { get; private set; }
        public bool Selected { get; private set; } = false;

        private void Start()
        {
            this.CheckInitializeRequired();
            SelectionIndicator.SetActive(false);
        }

        public void MoveUnit(Vector3 destination)
        {
            if (UnitMovementService == null) return;

            UnitMovementService.MoveUnit(destination);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void Select()
        {
            Selected = true;
            SelectionIndicator.SetActive(true);
        }

        public void DeSelect()
        {
            Selected = false;
            SelectionIndicator.SetActive(false);
        }
    }
}

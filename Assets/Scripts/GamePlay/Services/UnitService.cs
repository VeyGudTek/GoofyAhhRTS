using Source.Shared.Utilities;
using System;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class PositionDto
    {
        public Vector3 Position { get; set; }
        public float Radius { get; set; }
    }

    public class UnitService : MonoBehaviour
    {
        [SerializeField]
        [InitializationRequired]
        private UnitMovementService UnitMovementService;
        [SerializeField]
        [InitializationRequired]
        private GameObject SelectionIndicator;
        [InitializationRequired]
        private BoxCollider HitBox;

        private float Health { get; set; }
        private float? Speed { get; set; }
        public Guid PlayerId { get; set; } = Guid.Empty;
        public bool Selected { get; private set; } = false;

        private void Awake()
        {
            HitBox = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            this.CheckInitializeRequired();
            SelectionIndicator.SetActive(false);
            SetPosition();
        }

        private void SetPosition()
        {
            Vector3 newPos = this.transform.position;
            newPos.y += HitBox.size.y / 2f;
            this.transform.position = newPos;
        }

        public void MoveUnit(Vector3 destination)
        {
            if (UnitMovementService == null) return;

            UnitMovementService.MoveUnit(destination);
        }

        public PositionDto GetPosition()
        {
            return new PositionDto()
            {
                Position = this.transform.position,
                Radius = HitBox == null ? 0f : HitBox.size.x / 2f
            };
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

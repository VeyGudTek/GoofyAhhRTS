using Source.Shared.Utilities;
using System;
using UnityEngine;

namespace Source.GamePlay.Services.Unit
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
        private UnitAttackService UnitAttackService;
        [SerializeField]
        [InitializationRequired]
        private GameObject SelectionIndicator;
        [InitializationRequired]
        private BoxCollider HitBox;

        private float Health { get; set; }
        private UnitService Target { get; set; }

        public Guid PlayerId { get; set; } = Guid.Empty;
        public bool Selected { get; private set; } = false;

        private void Awake()
        {
            HitBox = GetComponent<BoxCollider>();
            this.CheckInitializeRequired();
            UnitAttackService.InjectDependencies(this, 2f, 5f);
        }

        private void Start()
        {
            SelectionIndicator.SetActive(false);
            SetPosition();
        }

        private void SetPosition()
        {
            Vector3 newPos = this.transform.position;
            newPos.y += HitBox.size.y / 2f;
            this.transform.position = newPos;
        }

        public void MoveUnit(Vector3 destination, float stoppingDistance, UnitService target)
        {
            if (UnitMovementService == null) return;

            UnitMovementService.MoveUnit(destination, stoppingDistance);
        }

        public float GetArea()
        {
            return HitBox == null ? 0f : HitBox.size.x * HitBox.size.z;
        }

        public PositionDto GetPosition()
        {
            return new PositionDto()
            {
                Position = this.transform.position,
                Radius = HitBox == null ? 0f : HitBox.size.x / 2f
            };
        }

        public void Damage(float damage)
        {
           Health -= damage;
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

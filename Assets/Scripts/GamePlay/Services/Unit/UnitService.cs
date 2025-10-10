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
        [SerializeField]
        [InitializationRequired]
        private HealthBarService HealthBarService;
        [InitializationRequired]
        [SerializeField]
        private BoxCollider HitBox;

        [InitializationRequired]
        private UnitManagerService UnitManagerService;
        private float MaxHealth { get; set; } = 50f;
        private float Health { get; set; } = 50f;
        public UnitService Target { get; private set; }
        public float Range { get; private set; } = 5f;
        public Guid PlayerId { get; private set; } = Guid.Empty;
        public bool Selected { get; private set; } = false;

        public void InjectDependencies(UnitManagerService unitManagerService, Guid playerId)
        {
            UnitManagerService = unitManagerService;
            PlayerId = playerId;
        }

        private void Awake()
        {
            if (UnitAttackService != null) 
                UnitAttackService.InjectDependencies(this, Range, 1f, 10f);
            if (UnitMovementService != null)
                UnitMovementService.InjectDependencies(this);
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

        public void CommandUnit(Vector3 destination, float stoppingDistance, UnitService target)
        {
            Target = target;

            if (UnitMovementService == null || Target != null) return;
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

        public void RemoveDestroyedUnit(UnitService unit)
        {
            Target = null;
            if (UnitAttackService != null) 
                UnitAttackService.RemoveUnitInRange(unit);
        }

        public void Damage(float damage)
        {
            Health -= damage;
            if (Health < 0f)
            {
                UnitManagerService.DestroyUnit(this);
                return;
            }
            HealthBarService.SetHealth(Health / MaxHealth);
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

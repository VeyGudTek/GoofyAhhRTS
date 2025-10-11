using Source.GamePlay.Static.Classes;
using Source.GamePlay.Static.ScriptableObjects;
using Source.Shared.Utilities;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Source.GamePlay.Services.Unit.Instance
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
        private HealthBarService HealthBarService;
        [InitializationRequired]
        [SerializeField]
        private BoxCollider HitBox;
        [SerializeField]
        [InitializationRequired]
        private GameObject SelectionIndicator;
        [SerializeField]
        [InitializationRequired]
        private MeshRenderer MeshRenderer;
        [SerializeField]
        private NavMeshAgent NavMeshAgent;

        [InitializationRequired]
        private UnitManagerService UnitManagerService;
        private float MaxHealth { get; set; } = 50f;
        private float Health { get; set; } = 50f;
        public UnitService Target { get; private set; }
        public float Range { get; private set; } = 2.5f;
        public Guid PlayerId { get; private set; } = Guid.Empty;
        public bool Selected { get; private set; } = false;

        public void InjectDependencies(UnitManagerService unitManagerService, Guid playerId, UnitData unitData)
        {
            UnitManagerService = unitManagerService;
            PlayerId = playerId;
            MaxHealth = unitData.MaxHealth;
            Health = MaxHealth;
            Range = unitData.Range;

            if (MeshRenderer != null)
                MeshRenderer.material = unitData.Material;
            if (UnitAttackService != null)
                UnitAttackService.InjectDependencies(this, unitData);
            if (UnitMovementService != null)
                UnitMovementService.InjectDependencies(this, HitBox == null ? 0f : HitBox.size.y, NavMeshAgent, unitData.Speed);
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
            if (target != null && target.PlayerId != PlayerId)
            {
                Target = target;
            }

            if (UnitMovementService != null && Target == null)
            {
                UnitMovementService.MoveUnit(destination, stoppingDistance);
            }
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

        public bool CanSeeTarget()
        {
            return CanSeeTarget(Target);
        }

        public bool CanSeeTarget(UnitService target)
        {
            if (target == null) return false;

            int layersToHit = LayerMask.GetMask(LayerNames.Obstacle);
            Vector3 direction = target.transform.position - transform.position;
            Vector3 origin = transform.position;
            if (Physics.Raycast(origin, direction, Mathf.Infinity, layersToHit))
            {
                return false;
            }
            return true;
        }

        public bool IsInRangeOfTarget()
        {
            if (Target == null) return false;

            float distanceToTarget = (transform.position - Target.transform.position).magnitude;
            return distanceToTarget <= Range;
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

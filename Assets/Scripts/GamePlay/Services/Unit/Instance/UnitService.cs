using Source.GamePlay.Static.Classes;
using Source.GamePlay.Static.ScriptableObjects;
using Source.Shared.Utilities;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Source.GamePlay.Services.Unit.Instance
{
    public enum UnitType
    {
        Regular,
        Harvestor,
        Home,
        Resource
    }

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
        private CapsuleCollider HitBox;
        [SerializeField]
        [InitializationRequired]
        private GameObject SelectionIndicator;
        [SerializeField]
        [InitializationRequired]
        private MeshRenderer MeshRenderer;
        [InitializationRequired]
        [SerializeField]
        private NavMeshAgent NavMeshAgent;
        [InitializationRequired]
        [SerializeField]
        private NavMeshObstacle navMeshObstacle;

        [InitializationRequired]
        private UnitManagerService UnitManagerService;
        public UnitService HomeBase { get; private set; }
        private float MaxHealth { get; set; } = 50f;
        private float Health { get; set; } = 50f;
        private UnitService Target { get; set; }
        public float Range { get; private set; } = 2.5f;
        public Guid PlayerId { get; private set; } = Guid.Empty;
        public bool Selected { get; private set; } = false;
        public bool HarvesterReturning { get; set; } = false;
        public UnitType UnitType { get; private set; } = UnitType.Regular;

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
                UnitMovementService.InjectDependencies(this, HitBox == null ? 0f : HitBox.height, NavMeshAgent, unitData.Speed);

            if (UnitType == UnitType.Home || UnitType == UnitType.Resource)
            {
                navMeshObstacle.enabled = true;
                NavMeshAgent.enabled = false;
            }
            else
            {
                navMeshObstacle.enabled = false;
                NavMeshAgent.enabled = true;                
            }
        }

        private void Start()
        {
            this.CheckInitializeRequired();
            SelectionIndicator.SetActive(false);
        }

        public void CommandUnit(Vector3 destination, float stoppingDistance, UnitService target)
        {
            if (target == null)
            {
                Target = null;

                if (UnitMovementService != null)
                {
                    UnitMovementService.MoveUnit(destination, stoppingDistance);
                }
            }
            else if (target.PlayerId != PlayerId)
            {
                Target = target;
            }
        }

        public float GetArea()
        {
            return HitBox == null ? 0f : Mathf.PI * (HitBox.radius * HitBox.radius);
        }

        public PositionDto GetPosition()
        {
            return new PositionDto()
            {
                Position = this.transform.position,
                Radius = HitBox == null ? 0f : HitBox.radius
            };
        }

        public bool CanSeeTarget()
        {
            return CanSeeUnit(CurrentTarget);
        }

        public bool CanSeeUnit(UnitService target)
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
            if (Target != null && unit == Target)
            {
                Target = null;
                if (UnitMovementService != null)
                    UnitMovementService.StopPathFinding();
            }
            
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

        public UnitService CurrentTarget => UnitType switch 
        { 
            UnitType.Harvestor => HarvesterReturning ? HomeBase : Target,
            UnitType.Regular => Target,
            _ => Target
        };
    }
}

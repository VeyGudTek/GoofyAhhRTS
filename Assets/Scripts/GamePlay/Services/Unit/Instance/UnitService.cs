using Source.GamePlay.Services.Unit.Instance.Types;
using Source.GamePlay.Static.Classes;
using Source.GamePlay.Static.ScriptableObjects;
using Source.Shared.Utilities;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Source.GamePlay.Services.Unit.Instance
{
    public class UnitService : MonoBehaviour
    {
        [SerializeField]
        [InitializationRequired]
        private UnitMovementService UnitMovementService;
        [SerializeField]
        [InitializationRequired]
        private UnitAttackService UnitAttackService;
        [field: SerializeField]
        [InitializationRequired]
        public UnitVisionService UnitVisionService { get; private set; }
        [field: SerializeField]
        [InitializationRequired]
        public UnitComputerService UnitComputerService { get; private set; }
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
        private UnitManagerService UnitManagerService { get; set; }
        [InitializationRequired]
        private ResourceService ResourceService { get; set; }
        [InitializationRequired]
        public BaseUnitTypeService UnitTypeService { get; private set; }
        private float MaxHealth { get; set; } = 50f;
        private float Health { get; set; } = 50f;
        public float Range { get; private set; } = 2.5f;
        public Guid PlayerId { get; private set; } = Guid.Empty;
        public bool Selected { get; private set; } = false;

        public void InjectDependencies(UnitManagerService unitManagerService, ResourceService resourceService, Guid playerId, UnitData unitData, int? computerId = null)
        {
            UnitManagerService = unitManagerService;
            ResourceService = resourceService;
            UnitTypeService = CreateUnitType(unitData.UnitType);
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
            if (UnitVisionService != null)
                UnitVisionService.InjectDependencies(this, unitData);
            if (UnitComputerService != null)
                UnitComputerService.InjectDependencies(this, computerId);
            if (UnitTypeService != null)
                UnitTypeService.InjectDependencies(this);

            if (!UnitTypeService.HasMove)
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

        private BaseUnitTypeService CreateUnitType(GameObject unitTypePrefab)
        {
            GameObject newUnitType = Instantiate(unitTypePrefab, transform);
            return newUnitType.GetComponent<BaseUnitTypeService>();
        }

        private void Start()
        {
            this.CheckInitializeRequired();
            SelectionIndicator.SetActive(false);
        }

        public void CommandUnit(Vector3 destination, float stoppingDistance, UnitService target, bool setComputer = true)
        {
            UnitTypeService.SetTarget(target);
            if (setComputer)
            {
                UnitComputerService.SetOriginalCommand(destination, stoppingDistance, target);
            }

            if (target == null)
            {
                UnitMovementService.MoveUnit(destination, stoppingDistance);
            }
        }

        public float Area => HitBox == null ? 0f : Mathf.PI * (HitBox.radius * HitBox.radius);
        public float Radius => HitBox == null ? 0f : HitBox.radius;
        public UnitService HomeBase => UnitManagerService.GetHomeBase(PlayerId);
        public int ComputerId => UnitComputerService.ComputerId;

        private bool CanSeeTarget()
        {
            return CanSeeUnit(UnitTypeService.GetTarget());
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

        private bool IsInRangeOfTarget()
        {
            if (UnitTypeService.GetTarget() == null) return false;

            float distanceToTarget = (transform.position - UnitTypeService.GetTarget().transform.position).magnitude;
            return distanceToTarget <= Range;
        }

        public bool CanAttackTarget => IsInRangeOfTarget() && CanSeeTarget();

        public void RemoveDestroyedUnit(UnitService unit)
        {
            if (UnitTypeService.OriginalTarget != null && UnitTypeService.OriginalTarget == unit)
            {
                UnitTypeService.SetTarget(null);
                UnitMovementService.StopPathFinding();
            }
            
            UnitAttackService.RemoveUnitInRange(unit);
            UnitVisionService.RemoveUnitInRange(unit);
            UnitComputerService.RemoveUnit(unit);
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

        public void AddGold(float gold)
        {
            ResourceService.ChangeResource(PlayerId, gold);
        }
    }
}

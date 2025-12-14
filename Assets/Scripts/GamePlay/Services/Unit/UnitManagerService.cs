using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Source.Shared.Utilities;
using Source.GamePlay.Static.Classes;
using Source.GamePlay.Services.Unit.Instance;
using Source.GamePlay.Static.ScriptableObjects;
using Source.GamePlay.Services.Unit.Computer;
using Source.GamePlay.Services.UI;

namespace Source.GamePlay.Services.Unit
{
    public class UnitManagerService : MonoBehaviour
    {
        [InitializationRequired]
        [SerializeField]
        private GameObject BaseUnit;
        [InitializationRequired]
        [SerializeField]
        private UnitService HomeUnit;
        [InitializationRequired]
        [SerializeField]
        private UnitService EnemyHomeUnit;
        
        [InitializationRequired]
        private UnitDataService UnitDataService { get; set; }
        [InitializationRequired]
        private GamePlayService GamePlayService { get; set; }
        [InitializationRequired]
        private ResourceService ResourceService { get; set; }
        [InitializationRequired]
        private UnitComputerManagerService UnitComputerManagerService { get; set; }
        [InitializationRequired]
        private SelectedUnitService SelectedUnitService { get; set; }

        private const float SpawnRayYOrigin = 100f;
        private readonly Vector3 SpawnOffset = new Vector3(1f, 0f, 0f);
        [SerializeField]
        private List<UnitService> ResourceUnits = new List<UnitService>();
        private readonly List<UnitService> Units = new();
        private readonly List<UnitService> PreviouslySelectedUnits = new();
        public IEnumerable<UnitService> AllyUnits => Units.Where(u => u.PlayerId == GamePlayService.PlayerId);

        public void InjectDependencies(UnitDataService unitDataService, 
            GamePlayService gamePlayService, 
            ResourceService resourceService, 
            UnitComputerManagerService unitComputerManagerService,
            SelectedUnitService selectedUnitService)
        {
            UnitDataService = unitDataService;
            GamePlayService = gamePlayService;
            ResourceService = resourceService;
            UnitComputerManagerService = unitComputerManagerService;
            SelectedUnitService = selectedUnitService;

            InitializeExistingUnits();
        }

        private void Start()
        {
            this.CheckInitializeRequired();

            InvokeRepeating(nameof(UpdateVisibleUnits), 0f, .5f);
        }

        private void InitializeExistingUnits()
        {
            if (GamePlayService == null || UnitDataService == null) return;

            HomeUnit.InjectDependencies(this, ResourceService, GamePlayService.PlayerId, UnitDataService.GetUnitData(Faction.ProCyber, UnitType.Home));
            Units.Add(HomeUnit);
            EnemyHomeUnit.InjectDependencies(this, ResourceService, GamePlayService.EnemyId, UnitDataService.GetUnitData(Faction.AntiCyber, UnitType.Home));
            Units.Add(EnemyHomeUnit);

            foreach(UnitService resource in  ResourceUnits)
            {
                resource.InjectDependencies(this, ResourceService, Guid.Empty, UnitDataService.GetUnitData(Faction.None, UnitType.Resource));
                Units.Add(resource);
            }
        }

        public void SpawnUnit(Guid playerId, Faction faction, UnitType type, int? computerId = null)
        {
            if (UnitDataService == null) return;

            int GroundLayer = LayerMask.GetMask(LayerNames.Ground);
            UnitService currentHomeUnit = playerId == GamePlayService.PlayerId ? HomeUnit : EnemyHomeUnit;
            Vector3 origin = new(currentHomeUnit.transform.position.x, SpawnRayYOrigin, currentHomeUnit.transform.position.z);
            UnitData unitData = UnitDataService.GetUnitData(faction, type);

            if (Physics.Raycast(origin + SpawnOffset, Vector3.down, out RaycastHit hit, Mathf.Infinity, GroundLayer))
            {
                GameObject newUnit = Instantiate(BaseUnit, hit.point, Quaternion.identity, this.transform);
                UnitService unitService = newUnit.GetComponent<UnitService>();
                Units.Add(unitService);
                unitService.InjectDependencies(this, ResourceService, playerId, unitData, computerId);
                unitService.CommandUnit(hit.point + SpawnOffset, unitService.Radius, null);

                ResourceService.ChangeResource(playerId, -unitData.Cost);
            }
        }

        public void SelectUnit(UnitService selectedUnit, bool deselectPrevious)
        {
            if (deselectPrevious)
            {
                DeSelectUnits(true);
            }
            
            if (selectedUnit != null)
            {
                if (selectedUnit.Selected)
                {
                    PreviouslySelectedUnits.Remove(selectedUnit);
                    selectedUnit.DeSelect();
                }
                else
                {
                    PreviouslySelectedUnits.Add(selectedUnit);
                    selectedUnit.Select();
                }
            }

            SelectedUnitService.OnNewSelection(AllyUnits.Where(u => u.Selected).ToList());
        }

        public void SelectUnits(List<UnitService> unitsToSelect)
        {
            foreach (UnitService unit in Units.Where(u => !PreviouslySelectedUnits.Contains(u) && !unitsToSelect.Contains(u)))
            {
                unit.DeSelect();
            }

            foreach (UnitService unit in unitsToSelect)
            {
                unit.Select();
            }

            SelectedUnitService.OnNewSelection(AllyUnits.Where(u => u.Selected).ToList());
        }

        public void DeSelectUnits(bool includePrevious)
        {
            if (includePrevious)
            {
                PreviouslySelectedUnits.Clear();
                foreach (UnitService unit in Units)
                {
                    unit.DeSelect();
                }
            }
            else
            {
                foreach (UnitService unit in Units.Where(u => !PreviouslySelectedUnits.Contains(u)))
                {
                    unit.DeSelect();
                }
            }
        }

        public void AddSelectedToPrevious()
        {
            foreach(UnitService unit in Units.Where(u => u.Selected && !PreviouslySelectedUnits.Contains(u)))
            {
                PreviouslySelectedUnits.Add(unit);
            }
        }

        public void MoveUnits(Guid playerId, Vector3 destination, UnitService target)
        {
            IEnumerable<UnitService> unitsToMove = Units.Where(u => 
                u.PlayerId == playerId &&
                u.Selected
            );

            MoveUnits(unitsToMove, destination, target);
        }

        public void MoveUnits(Vector3 destination, UnitService target, List<int> computerIds)
        {
            IEnumerable<UnitService> unitsToMove = Units.Where(u => computerIds.Contains(u.ComputerId));

            MoveUnits(unitsToMove, destination, target);
        }

        private void MoveUnits(IEnumerable<UnitService> unitsToMove, Vector3 destination, UnitService target)
        {
            float stoppingDistance = unitsToMove.Aggregate(0f,
                (total, currUnit) => total + currUnit.Area,
                total => Mathf.Sqrt(total / Mathf.PI)
            );

            foreach (UnitService unit in unitsToMove)
            {
                unit.CommandUnit(destination, stoppingDistance, target);
            }
        }

        public void DestroyUnit(UnitService unitToDestroy)
        {
            Units.Remove(unitToDestroy);
            UnitComputerManagerService.OnUnitDelete(unitToDestroy);
            PreviouslySelectedUnits.Remove(unitToDestroy);
            foreach(UnitService currentUnit in Units)
            {
                currentUnit.RemoveDestroyedUnit(unitToDestroy);
            }
            Destroy(unitToDestroy.gameObject);
        }

        public UnitService GetHomeBase(Guid playerId)
        {
            if (playerId == GamePlayService.PlayerId) return HomeUnit;
            if (playerId == GamePlayService.EnemyId) return EnemyHomeUnit;
            return null;
        }

        public int GetCountByComputerIds(List<int> computerIds)
        {
            return Units.Where(u => computerIds.Contains(u.ComputerId)).Count();
        }

        private void UpdateVisibleUnits()
        {
            List<UnitService> visibleUnits = new();
            foreach (UnitService allyUnit in Units.Where(u => u.PlayerId == GamePlayService.PlayerId))
            {
                visibleUnits.AddRange(allyUnit.UnitVisionService.VisibleUnits);
            }
            foreach (UnitService nonAllyUnit in Units.Where(u => u.PlayerId != GamePlayService.PlayerId))
            {
                if (visibleUnits.Contains(nonAllyUnit))
                {
                    nonAllyUnit.VisibilityService.SetVisability(true);
                }
                else
                {
                    nonAllyUnit.VisibilityService.SetVisability(false);
                }
            }
        }
    }
}


using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Source.Shared.Utilities;
using Source.GamePlay.Static.Classes;
using Source.GamePlay.Services.Unit.Instance;

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

        private const float SpawnRayYOrigin = 100f;
        [SerializeField]
        private List<UnitService> ResourceUnits = new List<UnitService>();
        private readonly List<UnitService> Units = new();
        private readonly List<UnitService> PreviouslySelectedUnits = new();

        public void InjectDependencies(UnitDataService unitDataService, GamePlayService gamePlayService)
        {
            UnitDataService = unitDataService;
            GamePlayService = gamePlayService;

            InitializeExistingUnits();
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        private void InitializeExistingUnits()
        {
            if (GamePlayService == null || UnitDataService == null) return;

            HomeUnit.InjectDependencies(this, HomeUnit, GamePlayService.PlayerId, UnitDataService.GetUnitData(Faction.ProCyber, UnitType.Home));
            Units.Add(HomeUnit);
            EnemyHomeUnit.InjectDependencies(this, EnemyHomeUnit, GamePlayService.EnemyGuid, UnitDataService.GetUnitData(Faction.AntiCyber, UnitType.Home));
            Units.Add(EnemyHomeUnit);

            foreach(UnitService resource in  ResourceUnits)
            {
                resource.InjectDependencies(this, null, Guid.Empty, UnitDataService.GetUnitData(Faction.None, UnitType.Resource));
                Units.Add(resource);
            }
        }

        public void SpawnUnit(Guid playerId, Vector2 spawnLocation, Faction faction, UnitType type)
        {
            if (UnitDataService == null) return;

            int layerMaskToHit = LayerMask.GetMask(LayerNames.Ground);
            Vector3 origin = new(spawnLocation.x, SpawnRayYOrigin, spawnLocation.y);

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, Mathf.Infinity, layerMaskToHit))
            {
                GameObject newUnit = Instantiate(BaseUnit, hit.point, Quaternion.identity, this.transform);
                UnitService unitService = newUnit.GetComponent<UnitService>();
                Units.Add(unitService);
                unitService.InjectDependencies(this, playerId == GamePlayService.PlayerId ? HomeUnit : EnemyHomeUnit, playerId, UnitDataService.GetUnitData(faction, type));
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

            float stoppingDistance = unitsToMove.Aggregate(0f, 
                (total, currUnit) => total + currUnit.Area, 
                total => Mathf.Sqrt(total / 4f)
            );

            foreach (UnitService unit in unitsToMove)
            {
                unit.CommandUnit(destination, stoppingDistance, target);
            }
        }

        public void DestroyUnit(UnitService unitToDestroy)
        {
            Units.Remove(unitToDestroy);
            PreviouslySelectedUnits.Remove(unitToDestroy);
            foreach(UnitService currentUnit in Units)
            {
                currentUnit.RemoveDestroyedUnit(unitToDestroy);
            }
            Destroy(unitToDestroy.gameObject);
        }
    }
}


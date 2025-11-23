using System.Collections.Generic;
using System.Linq;
using Source.GamePlay.Services.Unit.Instance;
using Source.GamePlay.Static.ScriptableObjects;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.GamePlay.Services.Unit.Computer
{
    public class UnitComputerService : MonoBehaviour
    {
        [InitializationRequired]
        private GamePlayService GamePlayService { get; set; }
        [InitializationRequired]
        private UnitManagerService UnitManagerService { get; set; }
        [InitializationRequired]
        private UnitDataService UnitDataService { get; set; }

        [field: SerializeField]
        public bool HasComputer { get; private set; } = false;
        [SerializeField]
        private List<ComputerActionCommand> ActionCommands = new();
        [SerializeField]
        private List<ComputerSpawnCommand> SpawnCommands = new();

        public void InjectDependencies(GamePlayService gamePlayService, UnitManagerService unitManagerService, UnitDataService unitDataService)
        {
            GamePlayService = gamePlayService;
            UnitManagerService = unitManagerService;
            UnitDataService = unitDataService;
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        public void OnUpdateResource(float currentResource)
        {
            
            ComputerSpawnCommand currentCommand = SpawnCommands.FirstOrDefault();
            if (currentCommand == null) return;

            UnitData newUnitData = UnitDataService.GetUnitData(currentCommand.Faction, currentCommand.Type);
            if (newUnitData.cost > currentResource) return;

            SpawnCommands.RemoveAt(0);
            UnitManagerService.SpawnUnit(GamePlayService.EnemyId, currentCommand.Faction, currentCommand.Type, currentCommand.ComputerId);
            
            ProcessActionCommands();
        }

        private void ProcessActionCommands()
        {
            ComputerActionCommand currentCommand = ActionCommands.FirstOrDefault();
            if (currentCommand == null) return;

            int numUnits = UnitManagerService.GetCountByComputerIds(currentCommand.ComputerIds);
            if (numUnits < currentCommand.UnitThreshold) return;

            Vector3 destination = currentCommand.Target.transform.position;
            UnitService target = currentCommand.Target.GetComponent<UnitService>();
            UnitManagerService.MoveUnits(destination, target, currentCommand.ComputerIds);

            ActionCommands.RemoveAt(0);  
        }
    }
}


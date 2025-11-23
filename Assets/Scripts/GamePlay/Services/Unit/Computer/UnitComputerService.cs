using System.Collections.Generic;
using System.Linq;
using Source.GamePlay.Services.Unit.Instance;
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
        private TimerService TimerService { get; set; }
        [SerializeField]
        private bool HasComputer = false;
        [SerializeField]
        private List<ComputerActionCommand> ActionCommands = new();
        [SerializeField]
        private List<ComputerSpawnCommand> SpawnCommands = new();

        public void InjectDependencies(GamePlayService gamePlayService, UnitManagerService unitManagerService, TimerService timerService)
        {
            GamePlayService = gamePlayService;
            UnitManagerService = unitManagerService;
            TimerService = timerService;
        }

        private void Start()
        {
            this.CheckInitializeRequired();

            if (HasComputer)
            {
                InvokeRepeating(nameof(ProcessSpawnCommands), 0f, .5f);
                InvokeRepeating(nameof(ProcessActionCommands), .1f, .5f);
            }
        }

        private void ProcessSpawnCommands()
        {
            ComputerSpawnCommand currentCommand = SpawnCommands.FirstOrDefault();

            if (currentCommand != null && currentCommand.Time <= TimerService.CurrentTime)
            {
                UnitManagerService.SpawnUnit(GamePlayService.EnemyId, currentCommand.Faction, currentCommand.Type, currentCommand.ComputerId);

                SpawnCommands.RemoveAt(0);
            }
        }

        private void ProcessActionCommands()
        {
            ComputerActionCommand currentCommand = ActionCommands.FirstOrDefault();

            if (currentCommand != null && currentCommand.Time <= TimerService.CurrentTime)
            {
                Vector3 destination = currentCommand.Target.transform.position;
                UnitService target = currentCommand.Target.GetComponent<UnitService>();
                UnitManagerService.MoveUnits(destination, target, currentCommand.ComputerIds);

                ActionCommands.RemoveAt(0);
            }
        }
    }
}


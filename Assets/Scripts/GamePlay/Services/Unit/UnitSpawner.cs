using Source.GamePlay.Static.ScriptableObjects;
using Source.Shared.Utilities;
using System;
using UnityEngine;

namespace Source.GamePlay.Services.Unit
{
    public class UnitSpawner : MonoBehaviour
    {
        [InitializationRequired]
        private ResourceService ResourceService { get; set; }
        [InitializationRequired]
        private UnitManagerService UnitManagerService { get; set; }
        [InitializationRequired]
        private GamePlayService GamePlayService { get; set; }

        public void InjectDependencies(GamePlayService gamePlayService, ResourceService resourceService, UnitManagerService unitManagerService)
        {
            ResourceService = resourceService;
            UnitManagerService = unitManagerService;
            GamePlayService = gamePlayService;
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        public void SpawnUnit(Guid playerId, Faction faction, UnitType type)
        {
            UnitData unitData = UnitManagerService.SpawnUnit(playerId, faction, type);
            
            if (unitData != null)
            {
                float newResources = ResourceService.ChangeResource(playerId, -unitData.cost);
                GamePlayService.UpdatePlayerResources(newResources);
            }
        }
    }
}


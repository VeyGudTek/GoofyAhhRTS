using Source.GamePlay.Services.UI;
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
        private UnitButtonsService UnitButtonsService { get; set; }

        public void InjectDependencies(ResourceService resourceService, UnitManagerService unitManagerService, UnitButtonsService unitButtonsService)
        {
            ResourceService = resourceService;
            UnitManagerService = unitManagerService;
            UnitButtonsService = unitButtonsService;
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
                float newResource = ResourceService.ChangeResource(playerId, -unitData.cost);
                UnitButtonsService.UpdateDisabledButtons(newResource);
            }
        }
    }
}


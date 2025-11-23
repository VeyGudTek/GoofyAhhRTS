using Source.GamePlay.Services.UI;
using Source.GamePlay.Services.Unit.Computer;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Source.GamePlay.Services
{
    public class ResourceService : MonoBehaviour
    {
        private GamePlayService GamePlayService { get; set; }
        private UnitButtonsService UnitButtonsService { get; set; }
        private UnitComputerService UnitComputerService { get; set; }
        [SerializeField]
        private Dictionary<Guid, float> ResourceMap = new();

        [SerializeField]
        private TMP_Text ResourceText;

        public void InjectDependencies(GamePlayService gamePlayService, UnitButtonsService unitButtonsService, UnitComputerService unitComputerService)
        {
            GamePlayService = gamePlayService;
            UnitButtonsService = unitButtonsService;
            UnitComputerService = unitComputerService;
        }

        private void Start()
        {
            ChangeResource(GamePlayService.PlayerId, 100);
            ChangeResource(GamePlayService.EnemyId, 20);
        }

        public void ChangeResource(Guid playerId, float value)
        {
            if (ResourceMap.ContainsKey(playerId))
            {
                ResourceMap[playerId] += value;
            }
            else
            {
                ResourceMap.Add(playerId, value);
            }
                
            if (playerId == GamePlayService.PlayerId)
            {
                UpdateResource(ResourceMap[playerId]);
            }
            if (playerId == GamePlayService.EnemyId && UnitComputerService.HasComputer)
            {
                UnitComputerService.OnUpdateResource(ResourceMap[playerId]);
            }
        }

        private void UpdateResource(float resource)
        {
            ResourceText.text = $"Resources: {resource.ToString()}";
            UnitButtonsService.UpdateDisabledButtons(resource);
        }
    }
}


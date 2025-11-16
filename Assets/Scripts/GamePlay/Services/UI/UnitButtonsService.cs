using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Source.GamePlay.Services.Unit;
using Source.GamePlay.Static.ScriptableObjects;

namespace Source.GamePlay.Services.UI
{
    public class UnitButtonsService : MonoBehaviour
    {
        [SerializeField]
        private List<Button> Buttons = new List<Button>();
        private Dictionary<int, float> ButtonCostMapping = new Dictionary<int, float>();
        private UnitSpawner UnitSpawner { get; set; }
        private GamePlayService GamePlayService { get; set; }
        private UnitDataService UnitDataService { get; set; }

        public void InjectDependencies(GamePlayService gamePlayService, UnitSpawner unitSpawner, UnitDataService unitDataService)
        {
            UnitSpawner = unitSpawner;
            GamePlayService = gamePlayService;
            UnitDataService = unitDataService;
            InitializeButtonInfo(0, Faction.ProCyber, UnitType.Regular);
        }

        public void InitializeButtonInfo(int buttonIndex, Faction faction, UnitType type)
        {
            Buttons[buttonIndex].onClick.AddListener(() => UnitSpawner.SpawnUnit(GamePlayService.PlayerId, faction, type));
            ButtonCostMapping.Add(buttonIndex, UnitDataService.GetUnitData(faction, type).cost);
        }

        public void UpdateDisabledButtons(float currentResources)
        {
            foreach(int buttonIndex in ButtonCostMapping.Keys)
            {
                if (ButtonCostMapping[buttonIndex] > currentResources)
                {
                    Buttons[buttonIndex].interactable = false;
                }
                else
                {
                    Buttons[buttonIndex].interactable = true;
                }
            }
        }
    }
}


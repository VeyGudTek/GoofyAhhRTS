using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using Source.GamePlay.Services.Unit;
using Source.GamePlay.Static.ScriptableObjects;

namespace Source.GamePlay.Services.UI
{
    public class UnitButtonsService : MonoBehaviour
    {
        [SerializeField]
        private List<Button> Buttons = new List<Button>();

        private UnitSpawner UnitSpawner { get; set; }
        private GamePlayService GamePlayService { get; set; }

        public void InjectDependencies(GamePlayService gamePlayService, UnitSpawner unitSpawner)
        {
            UnitSpawner = unitSpawner;
            GamePlayService = gamePlayService;
            InitializeButtonInfo();
        }

        private void InitializeButtonInfo()
        {
            Buttons[0].RegisterCallback<PointerDownEvent>(evt => UnitSpawner.SpawnUnit(GamePlayService.PlayerId, Faction.ProCyber, UnitType.Regular));
        }

        public void UpdateDisabledButtons(float currentResources)
        {

        }
    }
}


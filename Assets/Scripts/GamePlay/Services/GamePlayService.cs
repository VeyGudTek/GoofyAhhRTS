using Source.Shared.Services.Interfaces;
using Source.Shared.Utilities;
using Source.GamePlay.Services.Unit;
using System;
using UnityEngine;
using Source.GamePlay.Services.Unit.Instance;
using System.Collections.Generic;
using Source.Shared.Services;
using Source.GamePlay.Services.UI;
using Source.GamePlay.Static.ScriptableObjects;

namespace Source.GamePlay.Services
{
    internal enum GameState
    {
        Playing,
        Paused
    }

    public class GamePlayService: MonoBehaviour, IInputProcessorService
    {
        [InitializationRequired]
        private CameraService CameraService { get; set; }
        [InitializationRequired]
        private UnitManagerService UnitManagerService { get; set; }
        [InitializationRequired]
        private SelectionService SelectionService { get; set; }
        [InitializationRequired]
        PauseService PauseService { get; set; }
        [InitializationRequired]
        private SceneService SceneService { get; set; }

        public Guid PlayerId { get; private set; } = Guid.NewGuid();
        public Guid EnemyId { get; private set; } = Guid.NewGuid();
        private GameState GameState = GameState.Playing;

        public void InjectDependencies(CameraService cameraService, UnitManagerService unitManagerService, SelectionService selectionService,PauseService pauseService, SceneService sceneService)
        {
            CameraService = cameraService;
            UnitManagerService = unitManagerService;
            SelectionService = selectionService;
            PauseService = pauseService;
            SceneService = sceneService;
        }

        private void Start()
        {
            this.CheckInitializeRequired();
            //TempCodeBelow

            for (int i = -7; i < 7; i += 2)
            {
                UnitManagerService.SpawnUnit(PlayerId, new Vector2(i, -10), Faction.ProCyber, UnitType.Regular);
                UnitManagerService.SpawnUnit(PlayerId, new Vector2(i, -15), Faction.ProCyber, UnitType.Harvestor);
                UnitManagerService.SpawnUnit(EnemyId, new Vector2(10, i + 10), Faction.AntiCyber, UnitType.Regular);
            }
        }

        public void PrimaryClickEvent(bool isShift)
        {
            if (SelectionService == null) return;

            ContactDto contact = SelectionService.StartSelection();
            UnitManagerService.SelectUnit(contact.Unit, !isShift);
        }

        public void PrimaryHoldEvent()
        {
            if ( SelectionService == null || UnitManagerService == null) return;

            List<UnitService> selectedUnits = SelectionService.ContinueSelection();

            UnitManagerService.SelectUnits(selectedUnits);
        }

        public void PrimaryReleaseEvent() 
        {
            if (SelectionService == null || UnitManagerService == null) return;

            SelectionService.EndSelection();
            UnitManagerService.AddSelectedToPrevious();
        }

        public void SecondaryClickEvent() 
        { 
            if (SelectionService == null) return;

            ContactDto groundContact = SelectionService.GetGroundSelection();
            ContactDto unitContact = SelectionService.GetUnitSelection();
            if (!groundContact.HitGameObject) return;

            UnitManagerService.MoveUnits(PlayerId, groundContact.Point, unitContact.Unit);
        }

        public void SecondaryHoldEvent() { }
        public void SecondaryReleaseEvent() { }
        public void FixedMoveEvent(Vector2 moveVector)
        {
            CameraService.OnMove(moveVector);
        }

        public void CancelEvent()
        {
            if (PauseService == null) return;

            Action escapeAction = GameState switch
            {
                GameState.Playing => PauseGame,
                GameState.Paused => PauseService.ProcessCancel,
                _ => () => { }
            };

            escapeAction();
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
            PauseService.OpenPauseMenu();
            GameState = GameState.Paused;
        }

        public void UnPauseGame()
        {
            Time.timeScale = 1;
            GameState = GameState.Playing;
        }
    }
}

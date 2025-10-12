using Source.Shared.Services.Interfaces;
using Source.Shared.Utilities;
using Source.GamePlay.Services.Unit;
using System;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class GamePlayService: MonoBehaviour, IInputProcessorService
    {
        [InitializationRequired]
        private CameraService CameraService { get; set; }
        [InitializationRequired]
        private UnitManagerService UnitManagerService { get; set; }
        [InitializationRequired]
        private SelectionService SelectionService { get; set; }

        private Guid PlayerId { get; set; } = Guid.NewGuid();

        public void InjectDependencies(CameraService cameraService, UnitManagerService unitManagerService, SelectionService selectionService)
        {
            CameraService = cameraService;
            UnitManagerService = unitManagerService;
            SelectionService = selectionService;
        }

        private void Start()
        {
            this.CheckInitializeRequired();
            Guid enemyGuid = Guid.NewGuid();

            for (int i = -7; i < 7; i += 2)
            {
                UnitManagerService.SpawnUnit(PlayerId, new Vector2(i, -10), UnitType.Blue);
                UnitManagerService.SpawnUnit(enemyGuid, new Vector2(i, 20), UnitType.Red);
                UnitManagerService.SpawnUnit(enemyGuid, new Vector2(10, i + 10), UnitType.Red);
            }
        }

        public void PrimaryClickEvent(bool isShift)
        {
            if (SelectionService == null) return;

            ContactDto contact = SelectionService.StartSelection();
            UnitManagerService.SelectUnit(contact.Unit, !isShift);
        }

        public void PrimaryHoldEvent(bool isShift) 
        {
            if ( SelectionService == null || UnitManagerService == null) return;

            SelectionDto selection = SelectionService.ContinueSelection();

            if (selection.SuccessfulSelect)
            {
                UnitManagerService.SelectUnits(selection.Corner1, selection.Corner2, !isShift);
            }
            else
            {
                UnitManagerService.DeSelectUnits(false);
            }
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
        public void MoveEvent(Vector2 moveVector)
        {
            CameraService.OnMove(moveVector);
        }
    }
}

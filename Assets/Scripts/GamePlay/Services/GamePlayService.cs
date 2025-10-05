using Source.Shared.Services.Interfaces;
using Source.Shared.Utilities;
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

        public void InjectDependencies(CameraService cameraService, UnitManagerService unitManagerService, SelectionService selectionService)
        {
            CameraService = cameraService;
            UnitManagerService = unitManagerService;
            SelectionService = selectionService;
        }

        private void Start()
        {
            this.CheckInitializeRequired();

            for (int i = -7; i < 7; i++)
            {
                UnitManagerService.SpawnUnit(Guid.Empty, new Vector2(i, i));
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
            if ( SelectionService == null) return;

            SelectionDto selection = SelectionService.ContinueSelection();
            if ( !selection.SuccessfulSelect ) return;

            UnitManagerService.SelectUnits(Guid.Empty, selection.Corner1, selection.Corner2, !isShift);
        }

        public void PrimaryReleaseEvent() 
        {
            if (SelectionService == null) return;
            SelectionService.EndSelection();
        }

        public void SecondaryClickEvent() 
        { 
            if (SelectionService == null) return;

            ContactDto contact = SelectionService.GetGroundSelection();
            if (!contact.HitGameObject) return;

            UnitManagerService.MoveUnits(Guid.Empty, contact.Point);
        }

        public void SecondaryHoldEvent() { }
        public void SecondaryReleaseEvent() { }
        public void MoveEvent(Vector2 moveVector)
        {
            CameraService.OnMove(moveVector);
        }
    }
}

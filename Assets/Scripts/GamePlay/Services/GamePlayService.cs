using Source.Shared.Services.Interfaces;
using Source.Shared.Utilities;
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
        }

        public void PrimaryClickEvent()
        {
            if (SelectionService == null) return;

            ContactDto contact = SelectionService.StartSelection();
            Debug.Log($"Hit GameObject: {contact.HitGameObject} | WorldPoint: {contact.Point} | GameObject: {contact.GameObject?.name}");
        }

        public void PrimaryHoldEvent() 
        {
            if ( SelectionService == null) return;

            SelectionDto selection = SelectionService.ContinueSelection();
        }

        public void PrimaryReleaseEvent() 
        {
            if (SelectionService == null) return;
            SelectionService.EndSelection();
        }
        public void SecondaryClickEvent() { Debug.Log("SecondaryClick"); }
        public void SecondaryHoldEvent() { }
        public void SecondaryReleaseEvent() { }
        public void MoveEvent(Vector2 moveVector)
        {
            CameraService.OnMove(moveVector);
        }
    }
}

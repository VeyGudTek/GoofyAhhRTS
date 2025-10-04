using Source.Shared.Services.Interfaces;
using Source.Shared.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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

        public void InjectDependencies(CameraService cameraService, UnitManagerService unitManagerService)
        {
            CameraService = cameraService;
            UnitManagerService = unitManagerService;
        }

        private void Awake()
        {
            UnitManagerService = new UnitManagerService();
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        public void PrimaryClickEvent()
        {
            ContactDto contact = SelectionService.StartSelection();
            Debug.Log($"Hit GameObject: {contact.HitGameObject} | WorldPoint: {contact.Point} | GameObject: {contact.GameObject?.name}");
        }

        public void PrimaryHoldEvent() { }
        public void PrimaryReleaseEvent() { }
        public void SecondaryClickEvent() { Debug.Log("SecondaryClick"); }
        public void SecondaryHoldEvent() { }
        public void SecondaryReleaseEvent() { }
        public void MoveEvent(Vector2 moveVector)
        {
            CameraService.OnMove(moveVector);
        }
    }
}

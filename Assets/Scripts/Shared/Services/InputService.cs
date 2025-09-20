using Source.GamePlay.Services;
using Source.Shared.Controllers;
using Source.Shared.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Source.Shared.Services
{
    public class ContactDto
    {
        public bool HitGameObject = false;
        public Vector3? Point = null;
        public GameObject GameObject = null;
    }

    public class InputService
    {
        [InitializationRequired]
        private CameraService CameraService; //CHANGE THIS TO INTERFACE
        [InitializationRequired]
        private GamePlayService InputProcessorService; //CHANGE THIS TO GENERIC INTERFACE

        private IInputController InputController;

        public InputService(IInputController inputController)
        {
            InputController = inputController;
        }

        public void InjectDependencies(CameraService cameraService, GamePlayService gamePlayService)
        {
            CameraService = cameraService;
            InputProcessorService = gamePlayService;
        }

        public void OnUpdate()
        {
            UpdatePrimary();
            UpdateSecondary();
            UpdateMovement();
        }

        public ContactDto GetMouseWorldPoint()
        {
            ContactDto contact = new ContactDto();
            Vector2 mousePosition = Mouse.current.position.value;
            if (CameraService == null)
            {
                return contact;
            }

            if (MouseIsOverUI(mousePosition))
            {
                return contact;
            }
            
            Ray ray; 
            if (!CameraService.ScreenToWorldPoint(mousePosition, out ray))
            {
                return contact;
            }

            RaycastHit hit;
            int layerMaskToHit = LayerMask.GetMask("Default");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMaskToHit))
            {
                contact.HitGameObject = true;
                contact.Point = hit.point;
                contact.GameObject = hit.collider.gameObject;
            }

            return contact;
        }

        private bool MouseIsOverUI(Vector2 mousePosition)
        {
            int uiLayer = LayerMask.NameToLayer("UI");

            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Mouse.current.position.value;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);

            return raycastResults.Where(r => r.gameObject.layer == uiLayer).Count() > 0;
        }

        void UpdatePrimary()
        {
            if (InputController.PrimaryClicked())
            {
                InputProcessorService.PrimaryReleaseEvent();
            }
            if (InputController.PrimaryClicked())
            {
                InputProcessorService.PrimaryHoldEvent();
            }
            if (InputController.PrimaryClicked())
            {
                InputProcessorService.PrimaryClickEvent();
            }
        }

        void UpdateSecondary()
        {
            if (InputController.PrimaryClicked())
            {
                InputProcessorService.SecondaryReleaseEvent();
            }
            if (InputController.PrimaryClicked())
            {
                InputProcessorService.SecondaryHoldEvent();
            }
            if (InputController.PrimaryClicked())
            {
                InputProcessorService.SecondaryClickEvent();
            }
        }

        void UpdateMovement()
        {
            if (InputController.GetMove() != Vector2.zero)
            {
                InputProcessorService.MoveEvent();
            }
        }
    }
}

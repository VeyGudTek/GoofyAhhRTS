using Source.GamePlay.Services;
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

        private InputAction Primary; //CHANGE THIS TO INTERFACE
        private InputAction Secondary; //CHANGE THIS TO INTERFACE
        private InputAction Move; //CHANGE THIS TO INTERFACE

        public InputService(InputAction primary, InputAction secondary, InputAction move)
        {
            Primary = primary;
            Secondary = secondary;
            Move = move;
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

            int layerMaskToHit = LayerMask.GetMask("Default");
            RaycastHit hit;
            Ray? ray = CameraService.ScreenToWorldPoint(mousePosition);
            if (ray == null)
            {
                return contact;
            }

            if (Physics.Raycast((Ray)ray, out hit, Mathf.Infinity, layerMaskToHit))
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
            if (Primary.WasCompletedThisFrame())
            {
                InputProcessorService.PrimaryReleaseEvent();
            }
            if (Primary.IsInProgress())
            {
                InputProcessorService.PrimaryHoldEvent();
            }
            if (Primary.WasPressedThisFrame())
            {
                InputProcessorService.PrimaryClickEvent();
            }
        }

        void UpdateSecondary()
        {
            if (Secondary.WasCompletedThisFrame())
            {
                InputProcessorService.SecondaryReleaseEvent();
            }
            if (Secondary.IsInProgress())
            {
                InputProcessorService.SecondaryHoldEvent();
            }
            if (Secondary.WasPressedThisFrame())
            {
                InputProcessorService.SecondaryReleaseEvent();
            }
        }

        void UpdateMovement()
        {
            Vector2 moveVector = Move.ReadValue<Vector2>();
            if (moveVector != Vector2.zero)
            {
                InputProcessorService.MoveEvent();
            }
        }
    }
}

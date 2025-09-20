using Source.GamePlay.Services.Interfaces;
using Source.GamePlay.Services.Units;
using Source.GamePlay.Services.Units.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Source.GamePlay.Services
{
    public class ContactDto
    {
        public bool HitGameObject = false;
        public Vector3? Point = null;
        public GameObject GameObject = null;
    }

    public class GamePlayService: IGamePlayService
    {
        private ICameraService CameraService;
        private List<Unit> Units = new List<Unit>();
        private IUnitService UnitService;

        public void InjectDependencies(ICameraService cameraService, UnitService unitService)
        {
            CameraService = cameraService;
            UnitService = unitService;
        }

        public void PrimaryClickEvent()
        {
            ContactDto contact = GetMouseWorldPoint();
            if (contact != null)
            {
                Debug.Log($"Hit GameObject: {contact.HitGameObject} | WorldPoint: {contact.Point} | GameObject: {contact.GameObject?.name}");
            }
        }

        private ContactDto GetMouseWorldPoint()
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

        public void PrimaryHoldEvent() { }
        public void PrimaryReleaseEvent() { }
        public void SecondaryClickEvent() { }
        public void SecondaryHoldEvent() { }
        public void SecondaryReleaseEvent() { }
        public void MoveEvent(Vector2 moveVector)
        {
            CameraService.OnMove(moveVector);
        }
    }
}

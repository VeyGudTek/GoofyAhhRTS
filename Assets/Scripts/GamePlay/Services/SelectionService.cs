using Source.GamePlay.Services;
using Source.Shared.Utilities;
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
        public Vector3 Point = Vector3.zero;
        public GameObject GameObject = null;
    }

    public class SelectionDto
    {
        public bool SuccessfulSelect = false;
        public Vector3 Corner1 = Vector3.zero;
        public Vector3 Corner2 = Vector3.zero;
    }

    public class SelectionService : MonoBehaviour
    {
        [InitializationRequired]
        private GameObject SelectorObject { get; set; }

        [InitializationRequired]
        private CameraService CameraService { get; set; }

        private Vector3? StoredSelectionPoint { get; set; } = null;

        private const string UILayerName = "UI";
        private const string UnitLayerName = "Unit";
        private const string GroundLayerName = "Ground";

        private void Awake()
        {
            this.CheckInitializeRequired();
        }

        public void InjectDependencies(CameraService cameraService)
        {
            CameraService = cameraService;
        }

        public ContactDto StartSelection()
        {
            ContactDto groundContact = GetGroundSelection();
            if (groundContact.HitGameObject)
            {
                StoredSelectionPoint = groundContact.Point;
            }
            return GetUnitSelection();
        }

        public SelectionDto EndSelection()
        {
            SelectionDto selectionDto = new SelectionDto();
            if (StoredSelectionPoint == null) return selectionDto;

            ContactDto groundContact = GetGroundSelection();
            if (!groundContact.HitGameObject) return selectionDto;

            selectionDto.SuccessfulSelect = true;
            selectionDto.Corner1 = (Vector3)StoredSelectionPoint;
            selectionDto.Corner2 = groundContact.Point;

            return selectionDto;
        }

        private ContactDto GetUnitSelection()
        {
            int layerMaskToHit = LayerMask.GetMask(UnitLayerName);
            return GetMouseWorldPoint(layerMaskToHit);
        }

        private ContactDto GetGroundSelection()
        {
            int layerMaskToHit = LayerMask.GetMask(GroundLayerName);
            return GetMouseWorldPoint(layerMaskToHit);
        }

        private ContactDto GetMouseWorldPoint(int layerMaskToHit)
        {
            ContactDto contact = new ContactDto();
            Vector2 mousePosition = Mouse.current.position.value;
            Ray ray;
            RaycastHit hit;

            if (CameraService == null) return contact;
            if (MouseIsOverUI(mousePosition)) return contact;
            if (!CameraService.ScreenToWorldPoint(mousePosition, out ray)) return contact;
            
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
            int uiLayer = LayerMask.NameToLayer(UILayerName);

            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Mouse.current.position.value;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);

            return raycastResults.Where(r => r.gameObject.layer == uiLayer).Count() > 0;
        }
    }
}


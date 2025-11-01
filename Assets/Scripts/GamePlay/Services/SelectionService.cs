using Source.Shared.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Source.GamePlay.Services.Unit.Instance;
using Source.GamePlay.Static.Classes;
using Source.GamePlay.Utilities;

namespace Source.GamePlay.Services
{
    public class ContactDto
    {
        public bool HitGameObject = false;
        public Vector3 Point = Vector3.zero;
        public UnitService Unit = null;
    }

    public class SelectionService : MonoBehaviour
    {
        [InitializationRequired]
        [SerializeField]
        private GameObject SelectionIndicator;

        [InitializationRequired]
        private CameraService CameraService { get; set; }

        private Vector3? StoredSelectionPoint { get; set; } = null;
        private List<UnitService> SelectedUnits { get; set; } = new List<UnitService>();

        public void InjectDependencies(CameraService cameraService)
        {
            CameraService = cameraService;
        }

        private void Start()
        {
            this.CheckInitializeRequired();
            SelectionIndicator.SetActive(false);
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

        public List<UnitService> ContinueSelection()
        {
            ContactDto groundContact = GetGroundSelection();

            Vector3 storedSelection = StoredSelectionPoint == null ? groundContact.Point : (Vector3)StoredSelectionPoint;
            float length = Mathf.Abs((groundContact.Point.x - storedSelection.x));
            float width = Mathf.Abs((groundContact.Point.z - storedSelection.z));
            const float distanceThreshold = .05f;

            if (groundContact.HitGameObject) 
            {
                if (StoredSelectionPoint == null)
                {
                    StoredSelectionPoint = groundContact.Point;
                }
                UpdateTransform(groundContact.Point, length, width);

                if (length > distanceThreshold && width > distanceThreshold)
                {
                    SelectionIndicator.SetActive(true);
                    return SelectedUnits;
                }
            }
            SelectionIndicator.SetActive(false);
            return new List<UnitService>();
        }

        public void EndSelection()
        {
            StoredSelectionPoint = null;
            SelectionIndicator.SetActive(false);
        }

        public ContactDto GetUnitSelection()
        {
            int layerMaskToHit = LayerMask.GetMask(LayerNames.Unit);
            return GetMouseWorldPoint(layerMaskToHit);
        }

        public ContactDto GetGroundSelection()
        {
            int layerMaskToHit = LayerMask.GetMask(LayerNames.Ground);
            return GetMouseWorldPoint(layerMaskToHit);
        }

        private ContactDto GetMouseWorldPoint(int layerMaskToHit)
        {
            ContactDto contact = new();
            Vector2 mousePosition = Mouse.current.position.value;

            if (CameraService == null) return contact;
            if (MouseIsOverUI()) return contact;
            if (!CameraService.ScreenToWorldPoint(mousePosition, out Ray ray)) return contact;
            
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMaskToHit))
            {
                contact.HitGameObject = true;
                contact.Point = hit.point;
                contact.Unit = hit.collider.gameObject.GetComponent<UnitService>();
            }

            return contact;
        }

        private bool MouseIsOverUI()
        {
            int uiLayer = LayerMask.NameToLayer(LayerNames.UI);

            PointerEventData eventData = new(EventSystem.current)
            {
                position = Mouse.current.position.value
            };
            List<RaycastResult> raycastResults = new();
            EventSystem.current.RaycastAll(eventData, raycastResults);

            return raycastResults.Where(r => r.gameObject.layer == uiLayer).Count() > 0;
        }

        private void UpdateTransform(Vector3 currentPoint, float length, float width)
        {
            if (SelectionIndicator == null) return;
            const int SelectionY = 1;
            const int SelectionHeight = 10;
            const int SelectionIndicatorHeight = 1;

            Vector3 storedSelection = (Vector3)StoredSelectionPoint;

            float midX = (currentPoint.x + storedSelection.x) / 2;
            float midZ = (currentPoint.z + storedSelection.z) / 2;

            transform.position = new Vector3(midX, SelectionY, midZ);
            transform.localScale = new Vector3(length, SelectionHeight, width);
            SelectionIndicator.transform.position = new Vector3(midX, SelectionY, midZ);
            SelectionIndicator.transform.SetGlobalScale(new Vector3(length, SelectionIndicatorHeight, width));
        }

        private void OnTriggerEnter(Collider other)
        {
            UnitService newUnit = other.gameObject.GetComponent<UnitService>();
            if (newUnit != null)
            {
                SelectedUnits.Add(newUnit);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            UnitService newUnit = other.gameObject.GetComponent<UnitService>();
            if (newUnit != null)
            {
                SelectedUnits.Remove(newUnit);
            }
        }
    }
}


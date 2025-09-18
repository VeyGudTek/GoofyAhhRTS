using Source.Shared.Utilities;
using System;
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

    public class InitializeInputCallbackDTO
    {
        public Func<Camera> GetCamera = null;
        public Action PrimaryClickEvent = null;
        public Action PrimaryHoldEvent = null;
        public Action PrimaryReleaseEvent = null;
        public Action SecondaryClickEvent = null;
        public Action SecondaryHoldEvent = null;
        public Action SecondaryReleaseEvent = null;
        public Action<Vector2> MoveEvent = null;
    }

    public class InputService : MonoBehaviour
    {
        [InitializationRequired]
        private Func<Camera> GetCamera = null;

        [InitializationRequired]
        private InputAction Primary;
        [InitializationRequired]
        private Action PrimaryClickEvent = null;
        [InitializationRequired]
        private Action PrimaryHoldEvent = null;
        [InitializationRequired]
        private Action PrimaryReleaseEvent = null;

        [InitializationRequired]
        private InputAction Secondary;
        [InitializationRequired]
        private Action SecondaryClickEvent = null;
        [InitializationRequired]
        private Action SecondaryHoldEvent = null;
        [InitializationRequired]
        private Action SecondaryReleaseEvent = null;

        private InputAction Move;
        private Action<Vector2> MoveEvent = null;

        public void Initialize(InitializeInputCallbackDTO callbacks)
        {
            GetCamera = callbacks.GetCamera;
            PrimaryClickEvent = callbacks.PrimaryClickEvent;
            PrimaryHoldEvent = callbacks.PrimaryHoldEvent;
            PrimaryReleaseEvent = callbacks.PrimaryReleaseEvent;
            SecondaryClickEvent = callbacks.SecondaryClickEvent;
            SecondaryHoldEvent = callbacks.SecondaryHoldEvent;
            SecondaryReleaseEvent = callbacks.SecondaryReleaseEvent;
            MoveEvent = callbacks.MoveEvent;
        }

        void Awake()
        {
            Primary = InputSystem.actions.FindAction("Attack");
            Secondary = InputSystem.actions.FindAction("RightClick");
            Move = InputSystem.actions.FindAction("Move");
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        void Update()
        {
            UpdatePrimary();
            UpdateSecondary();
            UpdateMovement();
        }

        public ContactDto GetMouseWorldPoint()
        {
            ContactDto contact = new ContactDto();
            Vector2 mousePosition = Mouse.current.position.value;
            Camera camera = GetCamera?.Invoke();
            if (camera == null)
            {
                return contact;
            }

            if (MouseIsOverUI(mousePosition))
            {
                return contact;
            }

            int layerMaskToHit = LayerMask.GetMask("Default");
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(mousePosition);
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
            if (Primary == null)
            {
                return;
            }

            if (Primary.WasCompletedThisFrame())
            {
                PrimaryClickEvent?.Invoke();
            }
            if (Primary.inProgress)
            {
                PrimaryHoldEvent?.Invoke();
            }
            if (Primary.WasPressedThisFrame())
            {
                PrimaryReleaseEvent?.Invoke();
            }
        }

        void UpdateSecondary()
        {
            if (Secondary == null)
            {
                return;
            }

            if (Secondary.WasCompletedThisFrame())
            {
                SecondaryClickEvent?.Invoke();
            }
            if (Secondary.inProgress)
            {
                SecondaryHoldEvent?.Invoke();
            }
            if (Secondary.WasPressedThisFrame())
            {
                SecondaryReleaseEvent?.Invoke();
            }
        }

        void UpdateMovement()
        {
            if (Move == null)
            {
                return;
            }

            Vector2 moveVector = Move.ReadValue<Vector2>();
            if (moveVector != Vector2.zero)
            {
                MoveEvent?.Invoke(moveVector);
            }
        }
    }
}

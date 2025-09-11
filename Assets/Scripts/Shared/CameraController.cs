using Source.Shared.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Source.Shared
{
    public class CameraController : MonoBehaviour
    {
        [InitializationRequired]
        private Rigidbody Rigidbody;
        [InitializationRequired]
        private Camera Camera;

        private readonly float LinearDamping = 2f;

        void Awake()
        {
            Camera = GetComponent<Camera>();
            Rigidbody = GetComponent<Rigidbody>();
        }

        void Start()
        {
            this.CheckInitializeRequired();
            Rigidbody.linearDamping = LinearDamping;
        }

        public void OnMove(Vector2 direction)
        {
            if (Rigidbody != null)
            {
                Rigidbody.AddForce(new Vector3(direction.x, 0f, direction.y));
            }
        }

        public Camera GetCamera()
        {
            return Camera;
        }
    }
}


using UnityEngine;
using UnityEngine.InputSystem;
using Source.Shared.Utilities;

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

        public Vector3 GetMouseWorldPoint()
        {
            Vector3 worldPoint = Vector3.zero;

            int layersToHit = LayerMask.GetMask("Default");
            RaycastHit hit;
            Ray ray = Camera.ScreenPointToRay(Mouse.current.position.value);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToHit))
            {
                worldPoint = hit.point;
            }
            
            return worldPoint;
        }
    }
}


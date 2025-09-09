using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Shared
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody Rigidbody;

        [SerializeField]
        private Camera Camera;

        private readonly float LinearDamping = 2f;

        void Awake()
        {
            Camera = GetComponent<Camera>();
            Rigidbody = GetComponent<Rigidbody>();
        }

        void Start()
        {
            Rigidbody.linearDamping = LinearDamping;
        }

        public void OnMove(float x, float y)
        {
            Rigidbody.AddForce(new Vector3(x, 0f, y));
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


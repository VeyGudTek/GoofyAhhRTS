using Source.Shared.Utilities;
using UnityEngine;

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
            Vector3 velocity = (new Vector3(direction.x, 0f, direction.y)) * Time.deltaTime * 500f;

            if (Rigidbody != null)
            {
                Rigidbody.AddForce(velocity);
            }
        }

        public Camera GetCamera()
        {
            return Camera;
        }
    }
}


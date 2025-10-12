using Source.Shared.Utilities;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class CameraService: MonoBehaviour
    {
        [InitializationRequired]
        private Rigidbody Rigidbody { get; set; }
        [InitializationRequired]
        private Camera Camera { get; set; }

        private const float LINEAR_DAMPING = 2f;
        private const float CAMERA_SPEED = 750f;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Camera = GetComponent<Camera>();
        }

        private void Start()
        {
            this.CheckInitializeRequired();
            Rigidbody.linearDamping = LINEAR_DAMPING;
        }

        public bool ScreenToWorldPoint(Vector2 mousePosition, out Ray ray)
        {
            ray = new Ray();

            if (Camera == null) return false;

            ray = Camera.ScreenPointToRay(mousePosition);
            return true;
        }

        public void OnMove(Vector2 direction)
        {
            if (Rigidbody == null) return;

            Vector3 velocity = CAMERA_SPEED * Time.deltaTime * new Vector3(direction.x, 0f, direction.y);
            Rigidbody.AddForce(velocity);
        }
    }
}


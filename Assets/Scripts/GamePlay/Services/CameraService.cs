using Source.Shared.Utilities;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class CameraService: MonoBehaviour
    {
        [InitializationRequired]
        [SerializeField]
        private Rigidbody Rigidbody { get; set; }
        [InitializationRequired]
        [SerializeField]
        private Camera Camera { get; set; }

        private const float LINEAR_DAMPING = 2f;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Camera = GetComponent<Camera>();
            Rigidbody.linearDamping = LINEAR_DAMPING;
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        public bool ScreenToWorldPoint(Vector2 mousePosition, out Ray ray)
        {
            Ray? result = Camera.ScreenPointToRay(mousePosition);
            if (result == null)
            {
                ray = new Ray();
                return false;
            }
            ray = (Ray)result;
            return true;
        }

        public void OnMove(Vector2 direction)
        {
            Vector3 velocity = (new Vector3(direction.x, 0f, direction.y)) * Time.deltaTime * 500f;
            Rigidbody.AddForce(velocity);
        }
    }
}


using Source.GamePlay.Controllers.Interfaces;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.GamePlay.Controllers
{
    public class CameraController : MonoBehaviour, ICameraController
    {
        [InitializationRequired]
        [SerializeField]
        private Rigidbody Rigidbody;
        [InitializationRequired]
        [SerializeField]
        private Camera Camera;

        void Start()
        {
            this.CheckInitializeRequired();
        }

        public Ray? CameraScreenPointToRay(Vector2 mousePosition)
        {
            if (Camera != null)
            {
                return Camera.ScreenPointToRay(mousePosition);
            }
            return null;
        }

        public void RigidBodySetDamping(float dampingForce)
        {
            if (Rigidbody != null)
            {
                Rigidbody.linearDamping = dampingForce;
            }
        }

        public void RigidBodyAddForce(Vector3 velocity)
        {
            if (Rigidbody != null)
            {
                Rigidbody.AddForce(velocity);
            }
        }
    }
}


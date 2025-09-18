using System;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class CameraService
    {
        private Func<Vector2, Ray?> CameraScreenToWorldPoint;
        private Action<Vector3> RigidBodyAddForce = null;
        private const float LinearDamping = 2f;

        public CameraService(Action<Vector3> rigidBodyAddForce, Action<float> rigidBodySetDamping, Func<Vector2, Ray?> cameraScreenToWorldPoint)
        {
            RigidBodyAddForce = rigidBodyAddForce;
            rigidBodySetDamping(LinearDamping);

        }

        public Ray? ScreenToWorldPoint(Vector2 mousePosition)
        {
            return CameraScreenToWorldPoint(mousePosition);
        }

        public void OnMove(Vector2 direction)
        {
            Vector3 velocity = (new Vector3(direction.x, 0f, direction.y)) * Time.deltaTime * 500f;

            if (RigidBodyAddForce != null)
            {
                RigidBodyAddForce(velocity);
            }
        }
    }
}


using UnityEngine;

namespace Source.GamePlay.Controllers.Interfaces
{
    public interface ICameraHumbleObject
    {
        Ray? CameraScreenPointToRay(Vector2 mousePosition);
        void RigidBodySetDamping(float dampingForce);
        void RigidBodyAddForce(Vector3 velocity);
    }
}

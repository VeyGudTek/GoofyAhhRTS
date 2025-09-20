using Source.GamePlay.Controllers.Interfaces;
using System;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class CameraService
    {
        private ICameraController CameraController;
        private const float LinearDamping = 2f;

        public CameraService(ICameraController cameraController)
        {
            CameraController = cameraController;
            cameraController.RigidBodySetDamping(LinearDamping);
        }

        public Ray? ScreenToWorldPoint(Vector2 mousePosition)
        {
            return CameraController.CameraScreenPointToRay(mousePosition);
        }

        public void OnMove(Vector2 direction)
        {
            Vector3 velocity = (new Vector3(direction.x, 0f, direction.y)) * Time.deltaTime * 500f;
            CameraController.RigidBodyAddForce(velocity);
        }
    }
}


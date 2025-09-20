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

        public bool ScreenToWorldPoint(Vector2 mousePosition, out Ray ray)
        {
            Ray? result = CameraController.CameraScreenPointToRay(mousePosition);
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
            CameraController.RigidBodyAddForce(velocity);
        }
    }
}


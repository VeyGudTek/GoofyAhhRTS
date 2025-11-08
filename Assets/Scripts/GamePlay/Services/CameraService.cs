using Source.Shared.Repositories;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.GamePlay.Services
{
    public class CameraService: MonoBehaviour
    {
        [InitializationRequired]
        [SerializeField]
        private Rigidbody Rigidbody;
        [InitializationRequired]
        [SerializeField]
        private Camera Camera;
        [InitializationRequired]
        private SettingsRepository SettingsRepo { get; set; } = null;

        private const float LINEAR_DAMPING = 15f;

        private void Start()
        {
            this.CheckInitializeRequired();
            Rigidbody.linearDamping = LINEAR_DAMPING;
        }

        public void InjectDependencies(SettingsRepository settingsRepo)
        {
            SettingsRepo = settingsRepo;
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
            if (Rigidbody == null || SettingsRepo == null) return;

            Vector3 velocity = SettingsRepo.GetSettings().CameraSpeed * new Vector3(direction.x, 0f, direction.y);
            Rigidbody.AddForce(velocity);
        }
    }
}


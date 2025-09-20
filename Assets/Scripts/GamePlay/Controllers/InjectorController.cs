using Source.GamePlay.Services;
using Source.Shared.Controllers;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.GamePlay.Controllers
{
    public class InjectorController : MonoBehaviour
    {
        [InitializationRequired]
        [SerializeField]
        private CameraController CameraController;
        [InitializationRequired]
        [SerializeField]
        private InputController InputController;

        [InitializationRequired]
        private InjectorService InjectorService;

        private void Awake()
        {
            this.CheckInitializeRequired();
        }

        void Start()
        {
            InjectorService = new InjectorService(InputController.InputService, CameraController.CameraService);
        }
    }
}

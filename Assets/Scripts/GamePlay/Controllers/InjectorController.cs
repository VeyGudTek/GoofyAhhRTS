using Source.GamePlay.Services;
using Source.GamePlay.Services.Units;
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

        private InjectorService InjectorService;

        void Awake()
        {
            this.CheckInitializeRequired();
        }

        void Start()
        {
            InjectorService = new InjectorService(InputController.InputService, CameraController.CameraService, new GamePlayService(), new UnitService());
        }
    }
}

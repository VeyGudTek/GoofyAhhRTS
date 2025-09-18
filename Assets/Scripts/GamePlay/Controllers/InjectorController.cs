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
        private GamePlayService GamePlayService;
        [InitializationRequired]
        private InjectorService InjectorService;

        private void Awake()
        {
            InjectorService = new InjectorService();
            GamePlayService = new GamePlayService();
        }

        void Start()
        {
            this.CheckInitializeRequired();
            InjectorService.OnStart(InputController.InputService, GamePlayService, CameraController.CameraService);
        }
    }
}

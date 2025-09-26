using Source.GamePlay.Services;
using Source.GamePlay.Services.Units;
using Source.Shared.Controllers;
using Source.Shared.Services;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.GamePlay.Controllers
{
    public class InjectorHumbleObject : MonoBehaviour
    {
        [InitializationRequired]
        [SerializeField]
        private CameraHumbleObject CameraController;
        [InitializationRequired]
        [SerializeField]
        private InputHumbleObject InputController;

        private InjectorService InjectorService;

        void Awake()
        {
            this.CheckInitializeRequired();
        }

        void Start()
        {
            InjectorService = new InjectorService(
                new InputService(InputController), 
                new CameraService(CameraController), 
                new GamePlayService(), 
                new UnitService()
            );
        }

        private void Update()
        {
            InjectorService.OnUpdate();
        }
    }
}

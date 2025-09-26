using Source.GamePlay.Services;
using Source.Shared.HumbleObjects;
using Source.Shared.Services;
using Source.Shared.Utilities;
using UnityEngine;

namespace Source.GamePlay.HumbleObjects
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
                new UnitManagerService()
            );
        }

        private void Update()
        {
            InjectorService.OnUpdate();
        }
    }
}

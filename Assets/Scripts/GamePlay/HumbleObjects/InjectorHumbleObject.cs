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
        private CameraHumbleObject CameraHumbleObject;
        [InitializationRequired]
        [SerializeField]
        private InputHumbleObject InputHumbleObject;

        private InjectorService InjectorService;

        void Awake()
        {
            this.CheckInitializeRequired();
        }

        void Start()
        {
            InjectorService = new InjectorService(
                new InputService(InputHumbleObject), 
                new CameraService(CameraHumbleObject), 
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

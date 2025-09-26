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
            InjectorService = new InjectorService(InputHumbleObject, CameraHumbleObject);
        }

        private void Update()
        {
            InjectorService.OnUpdate();
        }
    }
}

using UnityEngine;
using Source.Shared.Services;
using Source.Shared.Utilities;
using Source.GamePlay.Services;
using System;

namespace Source.GamePlay
{
    public class GamePlayController : MonoBehaviour
    {
        private readonly Action EmptyAction = () => { };

        [SerializeField]
        [InitializationRequired]
        private InputService InputService;
        [SerializeField]
        [InitializationRequired]
        private GamePlayService GamePlayService;
        [SerializeField]
        [InitializationRequired]
        private CameraService CameraService;
        void Awake()
        {
            InjectDependencies();
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        private void InjectDependencies()
        {
            if (InputService != null && GamePlayService != null && CameraService != null)
            {
                InputService.Initialize(new()
                {
                    GetCamera = CameraService.GetCamera,
                    PrimaryClickEvent = GamePlayService.OnClick,
                    PrimaryHoldEvent = EmptyAction,
                    PrimaryReleaseEvent = EmptyAction,
                    SecondaryClickEvent = EmptyAction,
                    SecondaryHoldEvent = EmptyAction,
                    SecondaryReleaseEvent = EmptyAction,
                    MoveEvent = CameraService.OnMove
                });
            }

            if (GamePlayService != null && InputService != null)
            {
                GamePlayService.Initialize(InputService.GetMouseWorldPoint);
            }
        }
    }
}

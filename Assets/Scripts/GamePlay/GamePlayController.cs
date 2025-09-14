using UnityEngine;
using Source.Shared;
using Source.Shared.Utilities;
using System;

namespace Source.GamePlay
{
    public class GamePlayController : MonoBehaviour
    {
        private readonly Action EmptyAction = () => { };

        [SerializeField]
        [InitializationRequired]
        private InputManager InputManager;
        [SerializeField]
        [InitializationRequired]
        private GamePlayService GamePlayService;
        [SerializeField]
        [InitializationRequired]
        private CameraController CameraController;
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
            if (InputManager != null && GamePlayService != null && CameraController != null)
            {
                InputManager.Initialize(new()
                {
                    GetCamera = CameraController.GetCamera,
                    PrimaryClickEvent = GamePlayService.OnClick,
                    PrimaryHoldEvent = EmptyAction,
                    PrimaryReleaseEvent = EmptyAction,
                    SecondaryClickEvent = EmptyAction,
                    SecondaryHoldEvent = EmptyAction,
                    SecondaryReleaseEvent = EmptyAction,
                    MoveEvent = CameraController.OnMove
                });
            }

            if (GamePlayService != null && InputManager != null)
            {
                GamePlayService.Initialize(new()
                {
                    GetMouseWorldPoint = InputManager.GetMouseWorldPoint
                });
            }
        }
    }
}

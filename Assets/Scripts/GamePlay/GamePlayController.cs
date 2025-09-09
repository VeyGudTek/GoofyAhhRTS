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
        private InputManager InputManager;
        [SerializeField]
        private GamePlayService GamePlayService;
        [SerializeField]
        private CameraController CameraController;
        void Awake()
        {
            InitializationChecker.CheckMonoBehaviors(className: this.GetType().Name,
                new() { Name = "InputManager", Dependency = InputManager },
                new() { Name = "GamePlayService", Dependency = GamePlayService },
                new() { Name = "CameraController", Dependency = CameraController }
            );

            InjectDependencies();
        }

        private void InjectDependencies()
        {
            InputManager.Initialize(new()
            {
                PrimaryClickEvent = GamePlayService.OnClick,
                PrimaryHoldEvent = EmptyAction,
                PrimaryReleaseEvent = EmptyAction,
                SecondaryClickEvent = EmptyAction,
                SecondaryHoldEvent = EmptyAction,
                SecondaryReleaseEvent = EmptyAction,
                MoveEvent = CameraController.OnMove
            });

            GamePlayService.Initialize(new()
            {
                GetMouseWorldPoint = CameraController.GetMouseWorldPoint
            });
        }
    }
}

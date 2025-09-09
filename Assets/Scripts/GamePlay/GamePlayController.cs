using UnityEngine;
using Source.Shared;
using Source.Shared.Utilities;

namespace Source.GamePlay
{
    public class GamePlayController : MonoBehaviour
    {
        [SerializeField]
        private InputManager InputManager;
        [SerializeField]
        private GamePlayService GamePlayService;
        [SerializeField]
        private CameraController CameraController;
        void Awake()
        {
            InitializationChecker.CheckDependencies(className: name,
                InputManager,
                GamePlayService,
                CameraController
            );

            InjectDependencies();
        }

        private void InjectDependencies()
        {
            InputManager.Initialize(new()
            {
                PrimaryClickEvent = GamePlayService.OnClick,
                MoveEvent = CameraController.OnMove
            });

            GamePlayService.Initialize(new()
            {
                GetMouseWorldPoint = CameraController.GetMouseWorldPoint
            });
        }
    }
}

using UnityEngine;
using Source.Shared;

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
            InputManager.InitializeCallbacks(new()
            {
                PrimaryClickEvent = GamePlayService.OnClick,
                MoveEvent = CameraController.OnMove
            });
        }
    }
}

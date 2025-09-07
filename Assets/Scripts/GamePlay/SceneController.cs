using UnityEngine;
using Source.Shared;

namespace Source.GamePlay
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField]
        private InputManager InputManager;
        [SerializeField]
        private GamePlayService GamePlayService;
        void Awake()
        {
            InputManager.InitializeCallbacks(new()
            {
                PrimaryClickEvent = GamePlayService.OnClick
            });
        }
    }
}

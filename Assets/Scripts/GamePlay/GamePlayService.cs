using UnityEngine;

namespace Source.GamePlay
{
    public delegate Vector3 GetMouseWorldPointCallback();

    public class InitializeGamePlayCallbackDto
    {
        public GetMouseWorldPointCallback GetMouseWorldPoint;
    }

    public enum GameState
    {
        Paused,
        Playing
    }

    public class GamePlayService : MonoBehaviour
    {
        private bool Initialized = false;
        private GameState State;
        private GetMouseWorldPointCallback GetMouseWorldPoint;

        private void Start()
        {
            State = GameState.Playing;
        }

        public void InitializeCallbacks(InitializeGamePlayCallbackDto callbacks)
        {
            GetMouseWorldPoint = callbacks.GetMouseWorldPoint;
        }

        public void OnClick()
        {
            Debug.Log(GetMouseWorldPoint?.Invoke());
        }

        public void OnHold()
        {

        }

        public void OnRelease()
        {

        }
    }
}

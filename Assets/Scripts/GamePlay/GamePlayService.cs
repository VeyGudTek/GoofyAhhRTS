using Source.Shared.Utilities;
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

    public class GamePlayService : InitializationRequiredMonoBehavior<InitializeGamePlayCallbackDto>
    {
        private GameState State;
        private GetMouseWorldPointCallback GetMouseWorldPoint;

        private void Start()
        {
            State = GameState.Playing;
            CheckInitialized();
        }

        override public void Initialize(InitializeGamePlayCallbackDto callbacks)
        {
            Initialized = true;
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

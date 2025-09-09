using Source.Shared.Utilities;
using System;
using UnityEngine;

namespace Source.GamePlay
{
    public class InitializeGamePlayCallbackDto
    {
        public Func<Vector3> GetMouseWorldPoint;
    }

    public enum GameState
    {
        Paused,
        Playing
    }

    public class GamePlayService : MonoBehaviour
    {
        private GameState State;
        private Func<Vector3> GetMouseWorldPoint;

        private void Start()
        {
            InitializationChecker.CheckDelegates(className: this.GetType().Name,
                new InitializeCheckDTO<Delegate>() { Name = "GetMouseWorldPoint", Dependency = GetMouseWorldPoint }
            );

            State = GameState.Playing;
        }

        public void Initialize(InitializeGamePlayCallbackDto callbacks)
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

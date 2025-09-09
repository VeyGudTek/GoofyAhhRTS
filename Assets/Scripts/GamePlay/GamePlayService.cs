using UnityEngine;

namespace Source.GamePlay
{
    public enum GameState
    {
        Paused,
        Playing
    }

    public class GamePlayService : MonoBehaviour
    {
        public GameState State;

        private void Start()
        {
            State = GameState.Playing;
        }

        public void OnClick()
        {
            Debug.Log("Clicked");
        }

        public void OnHold()
        {

        }

        public void OnRelease()
        {

        }
    }
}

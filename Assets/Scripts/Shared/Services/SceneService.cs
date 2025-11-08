using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.Shared.Services
{
    public class SceneService : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneCoRoutine(sceneName));
        }

        private IEnumerator LoadSceneCoRoutine(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}

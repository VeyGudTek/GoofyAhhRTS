using UnityEngine;
using Source.Shared;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    void Awake()
    {
        inputManager.InitializeCallbacks(new() { });
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    InputAction Primary;
    InputAction Secondary;
    void Awake()
    {
        Primary = InputSystem.actions.FindAction("Attack");
        Secondary = InputSystem.actions.FindAction("RightClick");
    }

    void Update()
    {
        Debug.Log(Primary.ReadValue<float>());
    }
}

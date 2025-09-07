using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private delegate void CallbackFunction();

    private InputAction Primary;
    private CallbackFunction PrimaryClickEvent = null;
    private CallbackFunction PrimaryHoldEvent = null;
    private CallbackFunction PrimaryOffEvent = null;

    private InputAction Secondary;
    private CallbackFunction SecondaryClickEvent = null;
    private CallbackFunction SecondaryHoldEvent = null;
    private CallbackFunction SecondaryOffEvent = null;

    void Awake()
    {
        Primary = InputSystem.actions.FindAction("Attack");
        Secondary = InputSystem.actions.FindAction("RightClick");
    }


    void Update()
    {
        UpdatePrimary();
        UpdateSecondary();
    }

    void UpdatePrimary()
    {
        if (Primary.WasCompletedThisFrame())
        {
            PrimaryClickEvent?.Invoke();
        }
        if (Primary.inProgress)
        {
            PrimaryHoldEvent?.Invoke();
        }
        if (Primary.WasPressedThisFrame())
        {
            PrimaryOffEvent?.Invoke();
        }
    }

    void UpdateSecondary()
    {
        if (Secondary.WasCompletedThisFrame())
        {
            SecondaryClickEvent?.Invoke();
        }
        if (Secondary.inProgress)
        {
            SecondaryHoldEvent?.Invoke();
        }
        if (Secondary.WasPressedThisFrame())
        {
            SecondaryOffEvent?.Invoke();
        }
    }
}

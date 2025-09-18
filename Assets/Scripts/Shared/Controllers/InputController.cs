using Source.Shared.Services;
using Source.Shared.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Shared.Controllers
{
    public class InputController : MonoBehaviour
    {
        [InitializationRequired]
        private InputAction Primary;
        [InitializationRequired]
        private InputAction Secondary;
        [InitializationRequired]
        private InputAction Move;
        [InitializationRequired]
        public InputService InputService { get; private set; }

        private void Awake()
        {
            Primary = InputSystem.actions.FindAction("Attack");
            Secondary = InputSystem.actions.FindAction("RightClick");
            Move = InputSystem.actions.FindAction("Move");
            InputService = new InputService(Primary, Secondary, Move);
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        private void Update()
        {
            InputService.OnUpdate();
        }
    }
}
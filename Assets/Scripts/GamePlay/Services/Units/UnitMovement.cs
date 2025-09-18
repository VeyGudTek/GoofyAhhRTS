using Source.Shared.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace Source.GamePlay.Services.Units
{
    public class UnitMovement : MonoBehaviour
    {
        [InitializationRequired]
        private NavMeshAgent NavMeshAgent;
        [InitializationRequired]
        private float? Speed = null;

        private void Awake()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            this.CheckInitializeRequired();
            NavMeshAgent.speed = (float)Speed;
        }

        public void Initialize(float speed)
        {
            Speed = speed;
        }

        public void MoveUnit(Vector3 destination)
        {
            NavMeshAgent.SetDestination(destination);
        }
    }
}

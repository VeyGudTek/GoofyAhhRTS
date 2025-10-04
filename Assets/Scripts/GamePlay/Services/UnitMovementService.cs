using UnityEngine;
using UnityEngine.AI;

namespace Source.GamePlay.Services
{
    public class UnitMovementService : MonoBehaviour
    {
        private NavMeshAgent NavMeshAgent;
        private void Awake()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void MoveUnit(Vector3 destination)
        {
            if (NavMeshAgent != null)
            {
                NavMeshAgent.SetDestination(destination);
            }
        }

        public void SetSpeed(float? speed)
        {
            if (NavMeshAgent != null && speed != null)
            {
                NavMeshAgent.speed = (float)speed;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.AI;

namespace Source.GamePlay.Services
{
    public class UnitMovementService : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent NavMeshAgent;

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

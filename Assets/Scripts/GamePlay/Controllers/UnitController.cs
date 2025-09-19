using Source.GamePlay.Services.Units;
using Source.Shared.Utilities;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Source.GamePlay.Controllers
{
    public class UnitController : MonoBehaviour
    {
        [InitializationRequired]
        [SerializeField]
        private NavMeshAgent NavMeshAgent;
        [InitializationRequired]
        private Unit Unit;

        public void OnCreate(Guid playerId, float health, float? speed)
        {
            Unit = new Unit(playerId, health, speed, MoveUnit, GetPosition, SetSpeed);
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        private Vector3 GetPosition()
        {
            return this.transform.position;
        }

        private void MoveUnit(Vector3 destination)
        {
            if (NavMeshAgent != null)
            {
                NavMeshAgent.SetDestination(destination);
            }
        }

        private void SetSpeed(float? speed)
        {
            if (NavMeshAgent != null && speed != null)
            {
                NavMeshAgent.speed = (float)speed;
            }
        }
    }
}

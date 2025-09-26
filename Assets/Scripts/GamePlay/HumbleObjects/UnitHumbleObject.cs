using Source.GamePlay.HumbleObjects.Interfaces;
using Source.GamePlay.Services;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Source.GamePlay.Controllers
{
    public class UnitHumbleObject : MonoBehaviour, IUnitHumbleObject
    {
        [SerializeField]
        private NavMeshAgent NavMeshAgent;

        public Unit GetUnit(Guid playerId, float health, float? speed)
        {
            return new Unit(playerId, health, speed, this);
        }

        public Vector3 GetPosition()
        {
            return this.transform.position;
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

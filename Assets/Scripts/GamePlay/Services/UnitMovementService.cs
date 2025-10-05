using Source.Shared.Utilities;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Source.GamePlay.Services
{
    public class UnitMovementService : MonoBehaviour
    {
        private enum MovementType
        {
            None,
            NavMesh
        }

        [SerializeField]
        private MovementType Movement;

        [InitializationRequired]
        private NavMeshAgent NavMeshAgent;
        [InitializationRequired]
        private BoxCollider HitBox;

        private void Awake()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            HitBox = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            CustomCheckInitializeRequired();
            SetNavmesh();
        }

        private void CustomCheckInitializeRequired()
        {
            try
            {
                this.CheckInitializeRequired();
            }
            catch (Exception e)
            {
                if (Movement == MovementType.NavMesh && e.Message.Contains(typeof(NavMeshAgent).Name))
                {
                    throw e;
                }
            }
        }

        private void SetNavmesh()
        {
            if (NavMeshAgent == null || HitBox == null) return;
            NavMeshAgent.baseOffset = HitBox.size.y / 2f;
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

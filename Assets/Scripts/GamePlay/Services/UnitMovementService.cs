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
        private MovementType Movement { get; set; }

        [InitializationRequired]
        private NavMeshAgent NavMeshAgent { get; set; }
        [InitializationRequired]
        private BoxCollider HitBox { get; set; }

        private int BasePriority { get; set; } = 99;

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
            NavMeshAgent.stoppingDistance = HitBox.size.x / 2f;
        }

        private void Update()
        {
            CheckReachedPath();
        }

        private void CheckReachedPath()
        {
            if (NavMeshAgent == null) return;

            if (!NavMeshAgent.pathPending && NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance)
            {
                NavMeshAgent.ResetPath();
                NavMeshAgent.avoidancePriority = BasePriority;
            }
        }

        public void MoveUnit(Vector3 destination)
        {
            if (NavMeshAgent != null)
            {
                NavMeshAgent.SetDestination(destination);
                NavMeshAgent.avoidancePriority = BasePriority - 1;
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

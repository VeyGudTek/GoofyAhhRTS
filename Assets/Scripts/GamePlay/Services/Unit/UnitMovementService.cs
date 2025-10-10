using Source.Shared.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Source.GamePlay.Services.Unit
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
        [SerializeField]
        private NavMeshAgent NavMeshAgent;
        [InitializationRequired]
        [SerializeField]
        private BoxCollider HitBox;

        [InitializationRequired]
        private UnitService Self;
        private int BasePriority { get; set; } = 99;
        private bool CanRefreshPath = true;
        const float RefreshPathTime = .5f;

        public void InjectDependencies(UnitService self)
        {
            Self = self;
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
                if (Movement == MovementType.NavMesh || !e.Message.Contains(typeof(NavMeshAgent).Name))
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

        private void Update()
        {
            UpdatePathingUsingTarget();
            CheckReachedPath();
        }

        private void UpdatePathingUsingTarget()
        {
            if (Self == null || Self.Target == null) return;

            if (CanRefreshPath)
            {
                MoveUnit(Self.Target.gameObject.transform.position, 0);
                CanRefreshPath = false;
                StartCoroutine(RefreshPath());
            }
        }

        IEnumerator RefreshPath()
        {
            yield return new WaitForSeconds(RefreshPathTime);
            CanRefreshPath = true;
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

        public void MoveUnit(Vector3 destination, float stoppingDistance)
        {
            if (NavMeshAgent != null)
            {
                NavMeshAgent.SetDestination(destination);
                NavMeshAgent.avoidancePriority = BasePriority - 1;
                NavMeshAgent.stoppingDistance = stoppingDistance;
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

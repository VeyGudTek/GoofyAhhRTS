using Source.Shared.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Source.GamePlay.Services.Unit
{
    public class UnitMovementService : MonoBehaviour
    {
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
            this.CheckInitializeRequired();
            SetNavmesh();
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
            if (Self == null || Self.Target == null || (Self.IsInRangeOfTarget() && Self.CanSeeTarget()) || !CanRefreshPath) return;

            float stoppingDistance = (Self.GetPosition().Radius * 2) + Self.Target.GetPosition().Radius;
            
            MoveUnit(Self.Target.gameObject.transform.position, stoppingDistance);
            CanRefreshPath = false;
            StartCoroutine(RefreshPath());
        }

        IEnumerator RefreshPath()
        {
            yield return new WaitForSeconds(RefreshPathTime);
            CanRefreshPath = true;
        }

        private void CheckReachedPath()
        {
            if (NavMeshAgent == null) return;

            if (Self != null && Self.Target != null)
            {
                if (Self.CanSeeTarget() && Self.IsInRangeOfTarget())
                {
                    StopPathFinding();
                    return;
                }
            }
            if (!NavMeshAgent.pathPending && NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance)
            {
                StopPathFinding();
            }
        }

        private void StopPathFinding()
        {
            NavMeshAgent.ResetPath();
            NavMeshAgent.avoidancePriority = BasePriority;
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
    }
}

using Source.Shared.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Source.GamePlay.Services.Unit.Instance
{
    public class UnitMovementService : MonoBehaviour
    {
        [InitializationRequired]
        private NavMeshAgent NavMeshAgent { get; set; }
        [InitializationRequired]
        [SerializeField]
        private LineRenderer LineRenderer;

        [InitializationRequired]
        private UnitService Self;
        private int BasePriority { get; set; } = 99;
        private bool CanRefreshPath = true;
        const float RefreshPathTime = .5f;

        public void InjectDependencies(UnitService self, float hitBoxHeight, NavMeshAgent navMeshAgent, float speed)
        {
            Self = self;
            NavMeshAgent = navMeshAgent;
            if (NavMeshAgent != null)
            {
                NavMeshAgent.baseOffset = hitBoxHeight / 2f;
                NavMeshAgent.avoidancePriority = BasePriority;
                NavMeshAgent.speed = speed;
            }
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        private void Update()
        {
            if (Self == null || !Self.UnitTypeService.HasMove) return;

            UpdatePathingUsingTarget();
            CheckReachedPath();
            DrawPath();
        }

        private void UpdatePathingUsingTarget()
        {
            UnitService currentTarget = Self.UnitTypeService.GetTarget();
            if (currentTarget == null) return;

            if (!Self.CanAttackTarget && CanRefreshPath)
            {
                float stoppingDistance = Self.Radius + currentTarget.Radius;
                MoveUnit(currentTarget.gameObject.transform.position, stoppingDistance);
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
            if (NavMeshAgent == null || !NavMeshAgent.hasPath) return;

            if (Self != null && Self.UnitTypeService.GetTarget() != null)
            {
                if (Self.CanAttackTarget)
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

        public void StopPathFinding()
        {
            NavMeshAgent.ResetPath();
            NavMeshAgent.avoidancePriority = BasePriority;
        }

        private void DrawPath()
        {
            if (NavMeshAgent == null || LineRenderer == null) return;

            if (NavMeshAgent.hasPath && !NavMeshAgent.pathPending)
            {
                LineRenderer.enabled = true;

                int numCorners = NavMeshAgent.path.corners.Length;
                Vector3[] corners = new Vector3[numCorners];
                Array.Copy(NavMeshAgent.path.corners, corners, numCorners);

                if (Self.UnitTypeService.GetTarget() != null)
                {
                    corners[numCorners - 1] = new Vector3(
                        Self.UnitTypeService.GetTarget().transform.position.x,
                        corners[numCorners - 1].y,
                        Self.UnitTypeService.GetTarget().transform.position.z
                    );
                }

                LineRenderer.positionCount = numCorners;
                LineRenderer.SetPositions(corners);
            }
            else if (Self.UnitTypeService.GetTarget() == null || Self.CanAttackTarget) 
            {
                LineRenderer.enabled = false;
            }
        }

        public void MoveUnit(Vector3 destination, float stoppingDistance)
        {
            if (NavMeshAgent != null && Self.UnitTypeService.HasMove)
            {
                NavMeshAgent.SetDestination(destination);
                NavMeshAgent.avoidancePriority = BasePriority - 1;
                NavMeshAgent.stoppingDistance = stoppingDistance;
            }
        }
    }
}

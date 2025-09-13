using Source.Shared.Utilities;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Source.GamePlay
{
    public class Unit : MonoBehaviour
    {
        [InitializationRequired]
        private NavMeshAgent NavMeshAgent;

        private int Health;
        private int Range;

        [InitializationRequired]
        public Guid? Player { get; private set; } = null;
        public bool Selected { get; private set; } = false;

        private void Awake()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        public void MoveUnit(Vector3 destination)
        {
            NavMeshAgent.SetDestination(destination);
        }

        public void Select()
        {
            Selected = true;
        }

        public void Deselect()
        {
            Selected = false;
        }
    }
}

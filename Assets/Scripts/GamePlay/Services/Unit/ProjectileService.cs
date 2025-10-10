using Source.Shared.Utilities;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.GamePlay.Services.Unit
{
    public class ProjectileService : MonoBehaviour
    {
        [InitializationRequired]
        LineRenderer LineRenderer { get; set; }

        private const float ProjectileDuration = .5f;
        private const float Variance = .2f;

        private void Awake()
        {
            LineRenderer = GetComponent<LineRenderer>();
        }

        void Start()
        {
            this.CheckInitializeRequired();
            LineRenderer.positionCount = 2;
            LineRenderer.enabled = false;
        }

        public void CreateProjectile(Vector3 start, Vector3 end)
        {
            LineRenderer.enabled = true;
            LineRenderer.SetPosition(0, start + GetVector3Variance());
            LineRenderer.SetPosition(1, end + GetVector3Variance());
            StartCoroutine(RemoveProjectile());
        }

        private Func<Vector3> GetVector3Variance = () => new Vector3(
            Random.Range(-Variance, Variance), 
            Random.Range(-Variance, Variance), 
            Random.Range(-Variance, Variance)
        );

        IEnumerator RemoveProjectile()
        {
            yield return new WaitForSeconds(ProjectileDuration);
            LineRenderer.enabled = false;
        }
    }
}

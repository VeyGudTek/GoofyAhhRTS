using Source.Shared.Utilities;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.GamePlay.Services.Unit.Instance
{
    public class ProjectileService : MonoBehaviour
    {
        [InitializationRequired]
        [SerializeField]
        LineRenderer LineRenderer;

        private const float ProjectileDuration = .5f;
        private const float Variance = .2f;

        void Start()
        {
            this.CheckInitializeRequired();
            LineRenderer.positionCount = 2;
            LineRenderer.enabled = false;
        }

        public void SetProjectileColor(Color start, Color end)
        {
            if (LineRenderer != null)
            {
                LineRenderer.startColor = start;
                LineRenderer.endColor = end;
            }
        }

        public void CreateProjectile(Vector3 start, Vector3 end)
        {
            if (LineRenderer != null)
            {
                LineRenderer.enabled = true;
                LineRenderer.SetPosition(0, start + GetVector3Variance());
                LineRenderer.SetPosition(1, end + GetVector3Variance());
                StartCoroutine(RemoveProjectile());
            }
        }

        private readonly Func<Vector3> GetVector3Variance = () => new Vector3(
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

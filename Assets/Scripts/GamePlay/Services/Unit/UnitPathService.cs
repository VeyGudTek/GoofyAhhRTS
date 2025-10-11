using Source.Shared.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Source.GamePlay.Services.Unit
{
    public class UnitPathService : MonoBehaviour
    {
        [SerializeField]
        [InitializationRequired]
        LineRenderer LineRenderer;

        void Start()
        {
            this.CheckInitializeRequired();
        }

        public void DrawPath(List<Vector3> corners)
        {
            LineRenderer.SetPositions(corners.ToArray());
        }
    }
}


using UnityEngine;
using System.Collections.Generic;
using System;

namespace Source.GamePlay.Services
{
    public class ResourceService : MonoBehaviour
    {
        private Dictionary<Guid, float> Resources { get; set; } = new();

        public void CreateResourceMap(Guid id, float initialValue)
        {
            Resources.Add(id, initialValue);
        }

        public float GetResource(Guid id)
        {
            return Resources[id];
        }

        public float ChangeResource(Guid id, float value)
        {
            Resources[id] = Resources[id] + value;
            return Resources[id];
        }
    }
}


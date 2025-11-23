using System.Collections.Generic;
using System;
using UnityEngine;

namespace Source.GamePlay.Services.Unit
{
    [Serializable]
    public class ComputerActionCommand
    {
        public float Time;
        public List<int> ComputerIds;
        public GameObject Target;
    }
}
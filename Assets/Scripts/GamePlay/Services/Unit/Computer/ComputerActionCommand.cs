using System.Collections.Generic;
using System;
using UnityEngine;

namespace Source.GamePlay.Services.Unit
{
    [Serializable]
    public class ComputerActionCommand
    {
        public int UnitThreshold;
        public List<int> ComputerIds;
        public GameObject Target;
    }
}
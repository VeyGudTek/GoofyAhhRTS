using System.Collections.Generic;
using System;
using UnityEngine;
using Source.GamePlay.Services.Unit.Instance;

namespace Source.GamePlay.Services.Unit
{
    [Serializable]
    public class ComputerActionCommand
    {
        public int UnitThreshold;
        public UnitService UnitRequirement;
        public List<int> ComputerIds;
        public GameObject Target;
    }
}
using Source.GamePlay.Static.ScriptableObjects;
using System;

namespace Source.GamePlay.Services.Unit.Computer
{
    [Serializable]
    public class ComputerSpawnCommand
    {
        public float Time;
        public Faction Faction;
        public UnitType Type;
        public int ComputerId;
    }
}
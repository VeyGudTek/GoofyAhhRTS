using Source.GamePlay.Static.ScriptableObjects;
using System;

namespace Source.GamePlay.Services.Unit.Computer
{
    [Serializable]
    public class ComputerSpawnCommand
    {
        public Faction Faction;
        public UnitType Type;
        public int ComputerId;
    }
}
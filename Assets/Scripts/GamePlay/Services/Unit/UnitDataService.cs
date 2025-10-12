using Source.GamePlay.Static.ScriptableObjects;
using Source.Shared.Utilities;
using System;
using UnityEngine;

namespace Source.GamePlay.Services.Unit
{
    public enum UnitType
    {
        Blue,
        Red
    }

    public class UnitDataService : MonoBehaviour
    {
        [SerializeField]
        [InitializationRequired]
        private UnitData BlueUnitData;
        [SerializeField]
        [InitializationRequired]
        private UnitData RedUnitData;

        private void Awake()
        {
            this.CheckInitializeRequired();
        }

        public UnitData GetUnitData(UnitType color)
        {
            return color switch
            {
                UnitType.Blue => BlueUnitData,
                UnitType.Red => RedUnitData,
                _ => throw new Exception("[UnitDataService]: Invalid Unit Type"),
            };
        }
    }
}

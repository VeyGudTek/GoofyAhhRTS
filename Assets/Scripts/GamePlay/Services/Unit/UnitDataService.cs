using Source.GamePlay.Static.ScriptableObjects;
using Source.Shared.Utilities;
using System;
using UnityEngine;

namespace Source.GamePlay.Services.Unit
{
    public enum UnitColor
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

        public UnitData GetUnitData(UnitColor color)
        {
            return color switch
            {
                UnitColor.Blue => BlueUnitData,
                UnitColor.Red => RedUnitData,
                _ => throw new Exception("[UnitDataService]: Invalid Unit Type"),
            };
        }
    }
}

using Source.GamePlay.Services.Unit.Instance;
using Source.GamePlay.Static.ScriptableObjects;
using Source.Shared.Utilities;
using System;
using UnityEngine;

namespace Source.GamePlay.Services.Unit
{
    public enum Faction
    {
        ProCyber,
        AntiCyber,
        None
    }

    public class UnitDataService : MonoBehaviour
    {
        [SerializeField]
        [InitializationRequired]
        private UnitData BlueRegularData;
        [SerializeField]
        [InitializationRequired]
        private UnitData BlueHarvestorData;
        [SerializeField]
        [InitializationRequired]
        private UnitData BlueHomeData;
        [SerializeField]
        [InitializationRequired]
        private UnitData RedRegularData;
        [SerializeField]
        [InitializationRequired]
        private UnitData RedHarvestorData;
        [SerializeField]
        [InitializationRequired]
        private UnitData RedHomeData;
        [SerializeField]
        [InitializationRequired]
        private UnitData ResourceData;

        private void Awake()
        {
            this.CheckInitializeRequired();
        }

        public UnitData GetUnitData(Faction color, UnitType type)
        {
            if (type == UnitType.Resource)
                return ResourceData;

            UnitData data =  color switch
            {
                Faction.ProCyber => GetProCyberUnitData(type),
                Faction.AntiCyber => GetAntiCyberUnitData(type),
                _ => throw new Exception("[UnitDataService]: Invalid Unit Faction"),
            };

            return data == null ? new UnitData() : data;
        }

        public UnitData GetProCyberUnitData(UnitType type)
        {
            return type switch 
            {
                UnitType.Regular => BlueRegularData,
                UnitType.Home => BlueHomeData,
                UnitType.Harvestor => BlueHarvestorData,
                _ => throw new Exception("[UnitDataService]: Invalid Unit Type"),
            };
        }

        public UnitData GetAntiCyberUnitData(UnitType type)
        {
            return type switch
            {
                UnitType.Regular => RedRegularData,
                UnitType.Home => RedHomeData,
                UnitType.Harvestor => RedHarvestorData,
                _ => throw new Exception("[UnitDataService]: Invalid Unit Type"),
            };
        }
    }
}

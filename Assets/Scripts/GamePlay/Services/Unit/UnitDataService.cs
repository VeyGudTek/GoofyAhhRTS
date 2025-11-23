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
        private UnitData ProCyberRegularData;
        [SerializeField]
        [InitializationRequired]
        private UnitData ProCyberHarvestorData;
        [SerializeField]
        [InitializationRequired]
        private UnitData ProCyberHomeData;
        [SerializeField]
        [InitializationRequired]
        private UnitData AntiCyberRegularData;
        [SerializeField]
        [InitializationRequired]
        private UnitData AntiCyberHarvestorData;
        [SerializeField]
        [InitializationRequired]
        private UnitData AntiCyberHomeData;
        [SerializeField]
        [InitializationRequired]
        private UnitData ResourceData;

        private void Awake()
        {
            this.CheckInitializeRequired();
        }

        public UnitData GetUnitData(Faction faction, UnitType type)
        {
            if (type == UnitType.Resource)
                return ResourceData;

            UnitData data = faction switch
            {
                Faction.ProCyber => GetProCyberUnitData(type),
                Faction.AntiCyber => GetAntiCyberUnitData(type),
                _ => throw new Exception("[UnitDataService]: Invalid Unit Faction"),
            };

            return data == null ? new UnitData() : data;
        }

        private UnitData GetProCyberUnitData(UnitType type)
        {
            return type switch 
            {
                UnitType.Regular => ProCyberRegularData,
                UnitType.Home => ProCyberHomeData,
                UnitType.Harvestor => ProCyberHarvestorData,
                _ => throw new Exception("[UnitDataService]: Invalid Unit Type"),
            };
        }

        private UnitData GetAntiCyberUnitData(UnitType type)
        {
            return type switch
            {
                UnitType.Regular => AntiCyberRegularData,
                UnitType.Home => AntiCyberHomeData,
                UnitType.Harvestor => AntiCyberHarvestorData,
                _ => throw new Exception("[UnitDataService]: Invalid Unit Type"),
            };
        }
    }
}

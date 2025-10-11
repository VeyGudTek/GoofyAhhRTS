using Source.GamePlay.Static.ScriptableObjects;
using Source.Shared.Utilities;
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
            switch (color)
            {
                case UnitType.Blue:
                    return BlueUnitData != null ? BlueUnitData : new UnitData();
                default:
                    return RedUnitData != null ? RedUnitData : new UnitData();
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Source.GamePlay.Static.ScriptableObjects;
using UnityEngine;

namespace Source.GamePlay.Services.Unit.Instance
{
    public class UnitVisionService : MonoBehaviour
    {
        private UnitService Self { get; set; }
        private List<UnitService> UnitsInVisionRange { get; set; } = new();
        public IEnumerable<UnitService> VisibleUnits => UnitsInVisionRange.Where(u => Self.CanSeeUnit(u));

        public void InjectDependencies(UnitService self, UnitData unitData)
        {
            Self = self;

            transform.localScale = new Vector3(unitData.Vision * 2f, transform.localScale.y, unitData.Vision * 2f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Self == null) return;

            if (other.gameObject.TryGetComponent<UnitService>(out var newUnit))
            {
                UnitsInVisionRange.Add(newUnit);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (Self == null) return;

            if (other.gameObject.TryGetComponent<UnitService>(out var newUnit))
            {
                UnitsInVisionRange.Remove(newUnit);
            }
        }
        public void RemoveUnitInRange(UnitService unit)
        {
            UnitsInVisionRange.Remove(unit);
        }
    }
}


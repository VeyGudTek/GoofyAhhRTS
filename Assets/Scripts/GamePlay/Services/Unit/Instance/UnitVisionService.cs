using Source.GamePlay.Static.Classes;
using Source.GamePlay.Static.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.GamePlay.Services.Unit.Instance
{
    public class UnitVisionService : MonoBehaviour
    {
        private UnitService Self { get; set; }
        private List<UnitService> UnitsInVisionRange { get; set; } = new();
        public float VisionRange { get; private set; }
        public IEnumerable<UnitService> VisibleUnits => UnitsInVisionRange.Where(u => UnitUnobstructed(u));

        public void InjectDependencies(UnitService self, UnitData unitData)
        {
            Self = self;
            VisionRange = unitData.Vision;
            transform.localScale = new Vector3(unitData.Vision * 2f, transform.localScale.y, unitData.Vision * 2f);
        }

        private bool UnitUnobstructed(UnitService target)
        {
            int layersToHit = LayerMask.GetMask(LayerNames.Obstacle, LayerNames.Unit);
            Vector3 direction = target.transform.position - Self.transform.position;
            Vector3 origin = Self.transform.position;

            RaycastHit[] hits = Physics.RaycastAll(origin, direction, Mathf.Infinity, layersToHit);
            IEnumerable<GameObject> orderedObjects = hits
                .Select(h => h.collider.gameObject)
                .OrderBy(g => Vector3.Distance(g.transform.position, origin));

            foreach (GameObject obj in orderedObjects)
            {
                if (!obj.TryGetComponent(out UnitService unit))
                {
                    return false;
                }
                else if (unit = target)
                {
                    return true;
                }
            }
            return false;
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


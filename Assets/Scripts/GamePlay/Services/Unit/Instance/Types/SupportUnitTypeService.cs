using System.Collections.Generic;
using UnityEngine;

namespace Source.GamePlay.Services.Unit.Instance.Types
{
    public class SupportUnitTypeService : BaseUnitTypeService
    {
        private List<UnitService> unitsInRange = new List<UnitService>();
        public override bool HasMove => true;

        public override void Special()
        {
            foreach (UnitService unit in unitsInRange)
            {

            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out UnitService unit))
            {
                unitsInRange.Add(unit);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out UnitService unit))
            {
                unitsInRange.Remove(unit);
            }
        }
    }
}

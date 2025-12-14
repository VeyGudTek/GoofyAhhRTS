using System.Collections.Generic;
using UnityEngine;

namespace Source.GamePlay.Services.Unit.Instance.Types
{
    public class SupportUnitTypeService : BaseUnitTypeService
    {
        private const float BuffDuration = 5f;
        private List<UnitService> UnitsInRange = new List<UnitService>();
        public override bool HasMove => true;

        public override void Special()
        {
            foreach (UnitService unit in UnitsInRange)
            {
                unit.UnitStatusService.AddBuff(Buff.Damage, BuffDuration);
            }
            Self.Damage(50f);
        }

        public override void RemoveUnitInRange(UnitService unit)
        {
            UnitsInRange.Remove(unit);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out UnitService unit))
            {
                if (unit.PlayerId == Self.PlayerId && unit != Self)
                {
                    UnitsInRange.Add(unit);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out UnitService unit))
            {
                if (unit.PlayerId == Self.PlayerId)
                {
                    UnitsInRange.Remove(unit);
                }
            }
        }
    }
}

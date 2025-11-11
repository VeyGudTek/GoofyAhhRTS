using Source.Shared.Utilities;
using UnityEngine;

namespace Source.GamePlay.Services.Unit.Instance
{
    public class UnitHarvestorService : MonoBehaviour
    {
        [InitializationRequired]
        private UnitService Self { get; set; }
        [InitializationRequired]
        private UnitService HomeBase { get; set; }
        private bool HarvesterReturning { get; set; } = false;

        public void InjectDependencies(UnitService self, UnitService homeBase)
        {
            Self = self;
            HomeBase = homeBase;
        }

        public bool CanAttack => false;
            //Self.UnitType == UnitType.Harvestor && Self.CurrentTarget.UnitType == UnitType.Resource ||
            //Self.UnitType == UnitType.Harvestor && Self.CurrentTarget == HomeBase;

        public bool TryAttack(UnitService target, float damage)
        {
            //if (Self.UnitType != UnitType.Harvestor) return false;

            if (HarvesterReturning)
            {
                Self.AddGold(damage);
            }
            else
            {
                target.Damage(damage);
            }

            HarvesterReturning = !HarvesterReturning;
            return true;
        }
            
        public UnitService GetCurrentTarget(UnitService target)
        {
            return null;
            //return Self.UnitType switch
            //{
            //    UnitType.Harvestor => HarvesterReturning ? HomeBase : target,
            //    UnitType.Regular => target,
            //    _ => target
            //};
        }
    }
}


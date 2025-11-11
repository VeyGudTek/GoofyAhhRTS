using System;

namespace Source.GamePlay.Services.Unit.Instance.Types
{
    public class RegularUnitService : BaseUnitTypeService
    {
        public override bool HasMove => true;
        public override bool HasAttack => true;

        public override UnitService GetTarget()
        {
            return Target;
        }

        public override void SetTarget(UnitService target)
        {
            if (target == null)
            {
                Target = null;
                return;
            }

            if (target.PlayerId != Self.PlayerId)
            {
                Target = target;
            }
        }

        public override bool CanManualAttack()
        {
            return !Target.UnitTypeService.IsResource;
        }

        public override bool CanAutoAttack(UnitService target)
        {
            return target.PlayerId != Self.PlayerId && !target.UnitTypeService.IsResource;
        }

        public override void Attack(UnitService target, float damage)
        {
            target.Damage(damage);
        }
    }
}

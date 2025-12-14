using UnityEngine;

namespace Source.GamePlay.Services.Unit.Instance.Types
{
    public class VanguardUnitTypeService : BaseUnitTypeService
    {
        private const float ShieldHealth = 50f;
        public override bool HasAttack => true;
        public override bool HasMove => true;

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
            return !(Target == null || Target.UnitTypeService.IsResource);
        }

        public override bool CanAutoAttack(UnitService target)
        {
            return target.PlayerId != Self.PlayerId && !target.UnitTypeService.IsResource;
        }

        public override void Attack(UnitService target, float damage)
        {
            target.Damage(damage + Self.UnitStatusService.GetDamageBuff());
        }

        public override void Special()
        {
            Self.UnitStatusService.SetShield(ShieldHealth);
        }
    }
}


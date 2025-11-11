namespace Source.GamePlay.Services.Unit.Instance.Types
{
    public class RegularUnitService : BaseUnitTypeService
    {
        public override bool HasMove => true;
        public override bool HasAttack => true;

        public override void SetTarget(UnitService target)
        {
            if (target.PlayerId != Self.PlayerId)
            {
                Target = target;
            }
        }

        public override bool CanManualAttack(UnitService target)
        {
            return Self.CurrentTarget.UnitType != UnitType.Resource;
        }

        public override bool CanAutoAttack(UnitService target)
        {
            return target.PlayerId != Self.PlayerId && target.UnitType != UnitType.Resource;
        }

        public override void Attack(UnitService target, float damage)
        {
            target.Damage(damage);
        }
    }
}

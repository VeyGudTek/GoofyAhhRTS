namespace Source.GamePlay.Services.Unit.Instance.Types
{
    public class HarvestorUnitService : BaseUnitTypeService
    {
        private bool HarvesterReturning { get; set; } = false;
        public override bool HasMove => true;
        public override bool HasAttack => true;
        public override UnitService CurrentTarget => Self.UnitType switch
        {
            UnitType.Harvestor => HarvesterReturning ? Self.HomeBase : Target,
            UnitType.Regular => Target,
            _ => Target
        };

        public override void SetTarget(UnitService target)
        {
            if (target.PlayerId != Self.PlayerId)
            {
                Target = target;
            }
        }

        public override bool CanManualAttack(UnitService target)
        {
            return Self.UnitType == UnitType.Harvestor && Self.CurrentTarget.UnitType == UnitType.Resource ||
            Self.UnitType == UnitType.Harvestor && Self.CurrentTarget == Self.HomeBase;
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

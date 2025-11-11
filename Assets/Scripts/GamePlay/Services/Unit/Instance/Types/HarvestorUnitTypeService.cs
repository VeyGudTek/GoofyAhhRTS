namespace Source.GamePlay.Services.Unit.Instance.Types
{
    public class HarvestorUnitTypeService : BaseUnitTypeService
    {
        private bool HarvesterReturning { get; set; } = false;
        public override bool HasMove => true;
        public override bool HasAttack => true;

        public override UnitService GetTarget()
        {
            return HarvesterReturning ? Self.HomeBase : Target;
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
            UnitService target = GetTarget();
            return target != null && (target.UnitTypeService.IsResource || target == Self.HomeBase);
        }

        public override void Attack(UnitService target, float damage)
        {
            if (HarvesterReturning)
            {
                Self.AddGold(damage);
            }
            else
            {
                target.Damage(damage);
            }

            HarvesterReturning = !HarvesterReturning;
        }
    }
}

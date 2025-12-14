namespace Source.GamePlay.Services.Unit.Instance.Types
{
    public class HarvestorUnitTypeService : BaseUnitTypeService
    {
        private int ResourceBag = 0;
        public override bool HasMove => true;
        public override bool HasAttack => true;
        private const float ResourceDamageModifier = .1f;
        private const int MaxResources = 3;

        public override UnitService GetTarget()
        {
            return ResourceBag >= MaxResources && Target != null ? Self.HomeBase : Target;
        }

        public override void SetTarget(UnitService target)
        {
            if (target == null)
            {
                Target = null;
                return;
            }

            if (target.PlayerId != Self.PlayerId || target.UnitTypeService.IsHome)
            {
                Target = target;
            }
        }

        public override bool CanManualAttack()
        {
            return CanAttack(GetTarget());
        }

        public override bool CanAutoAttack(UnitService target)
        {
            return CanAttack(target);
        }

        private bool CanAttack(UnitService target)
        {
            bool enemyAndNonResource = target.PlayerId != Self.PlayerId && !target.UnitTypeService.IsResource;
            bool resourceAndHasSpace = target.UnitTypeService.IsResource && ResourceBag < MaxResources;
            bool homeAndHasResource = target.UnitTypeService.IsHome && ResourceBag > 0;

            return enemyAndNonResource || homeAndHasResource || resourceAndHasSpace;
        }

        public override void Attack(UnitService target, float damage)
        {
            if (target.UnitTypeService.IsHome)
            {
                Self.AddGold(damage * ResourceBag);
                ResourceBag = 0;
            }
            else
            {
                float modifiedDamage = target.UnitTypeService.IsResource ? damage : damage * ResourceDamageModifier + Self.UnitStatusService.GetDamageBuff();
                target.Damage(modifiedDamage);

                if (target.UnitTypeService.IsResource)
                {
                    ResourceBag += 1;
                }
            }
        }
    }
}

using Source.Shared.Utilities;
using UnityEngine;

namespace Source.GamePlay.Services.Unit.Instance.Types
{
    public abstract class BaseUnitTypeService : MonoBehaviour
    {
        [InitializationRequired]
        protected UnitService Self { get; set; }
        protected UnitService Target { get; set; }
        public UnitService OriginalTarget => Target;

        public void InjectDependencies(UnitService self)
        {
            Self = self;
        }

        public virtual bool IsResource => false;
        public virtual bool HasMove => false;
        public virtual bool HasAttack => false;
        public virtual UnitService GetTarget() { return null; }
        public virtual void SetTarget(UnitService target) { }
        public virtual bool CanManualAttack() { return false; }
        public virtual bool CanAutoAttack(UnitService target) { return false; }
        public virtual void Attack(UnitService target, float damage) { }
    }
}


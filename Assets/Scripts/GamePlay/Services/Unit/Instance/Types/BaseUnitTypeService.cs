using Source.Shared.Utilities;
using UnityEngine;

namespace Source.GamePlay.Services.Unit.Instance.Types
{
    public abstract class BaseUnitTypeService : MonoBehaviour
    {
        [InitializationRequired]
        internal UnitService Self { get; set; }
        internal UnitService Target { get; set; }

        public void InjectDependencies(UnitService self)
        {
            Self = self;
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        public virtual bool IsResource => false;
        public virtual bool HasMove => false;
        public virtual bool HasAttack => false;
        public virtual UnitService CurrentTarget => Target;
        public virtual void SetTarget(UnitService target) { }
        public virtual bool CanManualAttack(UnitService target) { return false; }
        public virtual bool CanAutoAttack(UnitService target) { return false; }
        public virtual void Attack(UnitService target, float damage) { }
    }
}


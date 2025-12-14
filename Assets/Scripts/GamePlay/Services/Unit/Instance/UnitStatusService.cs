using UnityEngine;

namespace Source.GamePlay.Services.Unit.Instance
{
    public class UnitStatusService : MonoBehaviour
    {
        private UnitService Self { get; set; }
        public float Shield { get; private set; } = 0f;
        public void InjectDependencies(UnitService self)
        {
            Self = self;
        }

        public void SetShield(float newValue)
        {
            Shield = newValue;
            Self.UnitVisualService.SetShield(Shield / Self.MaxHealth);
        }
    }
}
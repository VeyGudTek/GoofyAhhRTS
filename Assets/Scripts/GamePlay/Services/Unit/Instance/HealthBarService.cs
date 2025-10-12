using Source.Shared.Utilities;
using UnityEngine;

namespace Source.GamePlay.Services.Unit.Instance
{
    public class HealthBarService : MonoBehaviour
    {
        [SerializeField]
        [InitializationRequired]
        private GameObject HealthBar;
        private float BaseLength;

        void Start()
        {
            this.CheckInitializeRequired();
            BaseLength = HealthBar.transform.localScale.x;
        }

        public void SetHealth(float percentage)
        {
            if (HealthBar == null) return;

            percentage = Mathf.Min(percentage, 1);
            percentage = Mathf.Max(percentage, 0);
            float length = BaseLength * percentage;

            HealthBar.transform.localScale = new Vector3(
                length,
                HealthBar.transform.localScale.y,
                HealthBar.transform.localScale.z
            );
        }
    }
}

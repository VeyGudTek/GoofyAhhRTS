using System.Collections.Generic;
using UnityEngine;

namespace Source.GamePlay.Services.Unit.Instance
{
    public class UnitVisualService : MonoBehaviour
    {
        [SerializeField]
        private GameObject HealthBar;
        private float BaseHealthLength { get; set; }
        [SerializeField]
        private GameObject ShieldBar;
        [SerializeField]
        private GameObject DamageBuffIndicator;
        [SerializeField]
        private GameObject VisibilityIndicator;
        [SerializeField]
        private GameObject SelectionIndicator;
        [SerializeField]
        private MeshRenderer MeshRenderer;

        private void Start()
        {
            BaseHealthLength = HealthBar.transform.localScale.x;
            ShieldBar.SetActive(false);
            SelectionIndicator.SetActive(false);
            ShowBuffs(new List<Buff>());
        }

        public void SetMaterial(Material material)
        {
            MeshRenderer.material = material;
        }

        public void SetHealth(float percentage)
        {
            percentage = Mathf.Clamp(percentage, 0, 1);
            float length = BaseHealthLength * percentage;

            HealthBar.transform.localScale = new Vector3(
                length,
                HealthBar.transform.localScale.y,
                HealthBar.transform.localScale.z
            );
        }

        public void SetShield(float percentage)
        {
            if (percentage <= 0)
            {
                ShieldBar.SetActive(false);
                return;
            }

            ShieldBar.SetActive(true);
            float length = BaseHealthLength * percentage;

            ShieldBar.transform.localScale = new Vector3(
                length,
                ShieldBar.transform.localScale.y,
                ShieldBar.transform.localScale.z
            );
        }

        public void ShowBuffs(List<Buff> buffs)
        {
            DamageBuffIndicator.SetActive(buffs.Contains(Buff.Damage));
        }

        public void SetVisability(bool visible)
        {
            VisibilityIndicator.SetActive(visible);
        }

        public void SetSelect(bool visible)
        {
            SelectionIndicator.SetActive(visible);
        }
    }
}


using Source.Shared.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.GamePlay.Services.Unit
{
    public class UnitAttackService : MonoBehaviour
    {
        [InitializationRequired]
        private UnitService Self { get; set; }
        [InitializationRequired]
        [SerializeField]
        private ProjectileService ProjectileService;

        [SerializeField]
        private bool HasAttack;
        private float Cooldown { get; set; }
        private float Damage { get; set; }
        private bool CanAttack { get; set; } = true;

        private List<UnitService> UnitsInRange = new List<UnitService>();

        public void InjectDependencies(UnitService self, float range, float coolDown, float damage)
        {
            Self = self;
            Cooldown = coolDown;
            Damage = damage;
            transform.localScale = new Vector3(range, transform.localScale.y, range);
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        private void Update()
        {
            Attack();
        }

        private void Attack()
        {
            if (CanAttack && HasAttack && UnitsInRange.Count > 0)
            {
                UnitService target = UnitsInRange
                    .OrderBy(u => Mathf.Abs(u.gameObject.transform.position.magnitude - gameObject.transform.position.magnitude))
                    .First();
                
                target.Damage(Damage);
                CanAttack = false;

                if (ProjectileService != null)
                {
                    ProjectileService.CreateProjectile(transform.position, target.gameObject.transform.position);
                }

                StartCoroutine(ResetCooldown());
            }
        }

        IEnumerator ResetCooldown()
        {
            yield return new WaitForSeconds(Cooldown);
            CanAttack = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Self == null || HasAttack == false) return;
            UnitService newUnit = other.gameObject.GetComponent<UnitService>();

            if (newUnit != null && newUnit.PlayerId != Self.PlayerId)
            {
                UnitsInRange.Add(newUnit);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (Self == null || HasAttack == false) return;
            UnitService newUnit = other.gameObject.GetComponent<UnitService>();

            if (newUnit != null)
            {
                UnitsInRange.Remove(newUnit);
            }
        }

        public void RemoveUnitInRange(UnitService unit)
        {
            UnitsInRange.Remove(unit);
        }
    }
}

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
            transform.localScale = new Vector3(range * 2f, transform.localScale.y, range * 2f);
        }

        private void Start()
        {
            this.CheckInitializeRequired();
        }

        private void Update()
        {
            ProcessAttack();
        }

        private void ProcessAttack()
        {
            if (Self == null || Self.Target == null)
            {
                AutomaticAttack();
            }
            else
            {
                ManualAttack();
            }
        }

        private void AutomaticAttack()
        {
            IEnumerable<UnitService> visibleUnitsInRange = UnitsInRange.Where(u => Self.CanSeeTarget(u));
            if (CanAttack && HasAttack && visibleUnitsInRange.Count() > 0)
            {
                UnitService target = visibleUnitsInRange
                    .OrderBy(u => Mathf.Abs(u.gameObject.transform.position.magnitude - gameObject.transform.position.magnitude))
                    .First();

                AttackUnit(target);
            }
        }

        private void ManualAttack()
        {
            if (CanAttack && HasAttack && Self.CanSeeTarget() && Self.IsInRangeOfTarget())
            {
                AttackUnit(Self.Target);
            }
        }

        private void AttackUnit(UnitService target)
        {
            target.Damage(Damage);
            CanAttack = false;

            if (ProjectileService != null)
            {
                ProjectileService.CreateProjectile(transform.position, target.gameObject.transform.position);
            }

            StartCoroutine(ResetCooldown());
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

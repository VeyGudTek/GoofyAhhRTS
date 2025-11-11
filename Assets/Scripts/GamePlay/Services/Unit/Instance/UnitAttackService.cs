using Source.GamePlay.Static.ScriptableObjects;
using Source.Shared.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.GamePlay.Services.Unit.Instance
{
    public class UnitAttackService : MonoBehaviour
    {
        [InitializationRequired]
        private UnitService Self { get; set; }
        [InitializationRequired]
        [SerializeField]
        private ProjectileService ProjectileService;
        private float Cooldown { get; set; }
        private float Damage { get; set; }
        private bool CanAttack { get; set; } = true;

        private readonly List<UnitService> UnitsInRange = new();

        public void InjectDependencies(UnitService self, UnitData unitData)
        {
            Self = self;
            Cooldown = unitData.Cooldown;
            Damage = unitData.Damage;
            transform.localScale = new Vector3(unitData.Range * 2f, transform.localScale.y, unitData.Range * 2f);

            if (ProjectileService != null)
                ProjectileService.SetProjectileColor(unitData.ProjectileStartColor, unitData.ProjectileEndColor);
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
            if (Self == null) return;

            if (Self.UnitTypeService.GetTarget() == null)
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
            IEnumerable<UnitService> visibleUnitsInRange = UnitsInRange.Where(u => Self.CanSeeUnit(u));
            if (CanAttack && visibleUnitsInRange.Count() > 0)
            {
                UnitService target = visibleUnitsInRange
                    .OrderBy(u => (u.transform.position - Self.transform.position).sqrMagnitude)
                    .First();

                AttackUnit(target);
            }
        }

        private void ManualAttack()
        {
            if (CanAttack && Self.CanAttackTarget && Self.UnitTypeService.CanManualAttack())
            {
                AttackUnit(Self.UnitTypeService.GetTarget());
            }
        }

        private void AttackUnit(UnitService target)
        {
            Self.UnitTypeService.Attack(target, Damage);

            CanAttack = false;
            StartCoroutine(ResetCooldown());

            if (ProjectileService != null)
                ProjectileService.CreateProjectile(transform.position, target.gameObject.transform.position);
        }

        IEnumerator ResetCooldown()
        {
            yield return new WaitForSeconds(Cooldown);
            CanAttack = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Self == null) return;

            UnitService newUnit = other.gameObject.GetComponent<UnitService>();

            if (newUnit != null && newUnit.PlayerId != Self.PlayerId && Self.UnitTypeService.CanAutoAttack(newUnit))
            {
                UnitsInRange.Add(newUnit);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (Self == null) return;
            
            if (other.gameObject.TryGetComponent<UnitService>(out var newUnit))
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

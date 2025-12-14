using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Source.GamePlay.Services.Unit.Instance
{
    public enum Buff
    {
        Damage
    }

    public class UnitStatusService : MonoBehaviour
    {
        private List<Buff> Buffs = new List<Buff>();
        private const float DamageBuff = 10f;
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

        public void AddBuff(Buff buff, float duration)
        {
            Buffs.Add(buff);
            StartCoroutine(RemoveBuff(buff, duration));
        }

        private IEnumerator RemoveBuff(Buff buff, float duration)
        {
            yield return new WaitForSeconds(duration);
            Buffs.Remove(buff);
        }

        public float GetDamageBuff()
        {
            if (Buffs.Contains(Buff.Damage))
            {
                return DamageBuff;
            }
            return 0f;
        }
    }
}
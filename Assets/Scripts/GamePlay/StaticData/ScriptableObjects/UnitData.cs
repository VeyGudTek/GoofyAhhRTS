using Source.GamePlay.Services.Unit.Instance;
using UnityEngine;

namespace Source.GamePlay.Static.ScriptableObjects
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
    public class UnitData : ScriptableObject
    {
        public Material Material;
        public float MaxHealth;
        public float Range;
        public float Speed;
        public float Cooldown;
        public float damage;
        public Color ProjectileStartColor;
        public Color ProjectileEndColor;
        public UnitType UnitType;
    }
}

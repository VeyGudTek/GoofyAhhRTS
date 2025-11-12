using UnityEngine;

namespace Source.GamePlay.Static.ScriptableObjects
{
    public enum UnitType
    {
        Harvestor,
        Regular,
        Home,
        Resource
    }

    [CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
    public class UnitData : ScriptableObject
    {
        public Material Material;
        public float MaxHealth;
        public float Range;
        public float Speed;
        public float Cooldown;
        public float Damage;
        public Color ProjectileStartColor;
        public Color ProjectileEndColor;
        public UnitType UnitType;
    }
}

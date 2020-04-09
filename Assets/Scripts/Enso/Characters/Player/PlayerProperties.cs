using UnityEngine;

namespace Enso.Characters.Player
{
    [CreateAssetMenu(fileName = "HiroyukiProperties", menuName = "Enso/Hiroyuki")]
    public class PlayerProperties : FighterProperties
    {
        [Header("Movement")]
        public float MoveSpeed;
        public float SprintSpeed;
        public float MoveSpeedWhileDefending;
        public float AccelerationTime;
        public float DeadZone;

        [Header("Healing")] 
        public int HealingCharges;
        public float DelayToHealAgain;

        [Header("Dodge Roll")] 
        public float SlidingSpeed;
        public float SlidingLength;

        [Header("Simple Attack")]
        public Vector2 AttackRange;
        public int SimpleAttackDamage;

        [Header("Heavy Attack")]
        public Vector2 HeavyAttackRange;
        public int HeavyAttackDamage;
        public float HeavyAttackHoldingTime;
    }
}
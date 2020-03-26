using UnityEngine;

namespace Enso.Characters.Player
{
    [CreateAssetMenu(fileName = "PlayerProperties", menuName = "Enso/Player")]
    public class PlayerProperties : FighterProperties
    {
        [Header("Movement")]
        public float MoveSpeed;
        public float MoveSpeedWhileDefending;
        public float AccelerationTime;
        public float DeadZone;

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
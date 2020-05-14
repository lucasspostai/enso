using UnityEngine;

namespace Enso.Characters
{
    public class FighterProperties : ScriptableObject
    {
        [Header("Locomotion")]
        public float WalkSpeed;
        public float RunSpeed;
        public float SprintSpeed;
        public float GuardSpeed;
        public float AccelerationTime;
        public float DeadZone;
        
        [Header("Attributes")] 
        public int Health = 3;
        public float BalanceAmount = 10f;
        public float DelayToRecoverAfterLosingBalance = 5f;
        public float DelayToRecoverAfterDamage = 2f;
        public float TimeToFullyRecoverBalance = 10f;
    }
}

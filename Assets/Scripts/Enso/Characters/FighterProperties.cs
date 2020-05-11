using UnityEngine;

namespace Enso.Characters
{
    public class FighterProperties : ScriptableObject
    {
        [Header("Attributes")] 
        public int Health = 3;
        public float BalanceAmount = 10f;
        public float DelayToRecoverAfterLosingBalance = 5f;
        public float DelayToRecoverAfterDamage = 2f;
        public float TimeToFullyRecoverBalance = 10f;
    }
}

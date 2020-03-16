using UnityEngine;

namespace Gameplay
{
    public class FighterProperties : ScriptableObject
    {
        [Header("Attributes")] 
        public int Health;
        public int BalanceAmount;
        public float InvincibilityTime;
    }
}

using UnityEngine;

namespace Enso.Characters.Player
{
    [CreateAssetMenu(fileName = "HiroyukiProperties", menuName = "Enso/Hiroyuki")]
    public class PlayerProperties : FighterProperties
    {
        [Header("Healing")] 
        public int HealingCharges;
        public float DelayToHealAgain;
    }
}
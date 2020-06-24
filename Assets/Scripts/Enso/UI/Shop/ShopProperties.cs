using UnityEngine;

namespace Enso.UI.Shop
{
    [CreateAssetMenu(fileName = "ShopProperties", menuName = "Enso/Shop Properties")]
    public class ShopProperties : ScriptableObject
    {
        [Header("Extra Health")] 
        public int ExtraHealthCost;
        public int MaxHealth;

        [Header("Extra Balance")] 
        public int ExtraBalanceCost;
        public int MaxBalance;

        [Header("Extra Healing Charge")] 
        public int ExtraHealingChargeCost;
        public int MaxHealingCharges;

        [Header("Strong Attack")] 
        public int StrongAttackCost;

        [Header("Special Attack")] 
        public int SpecialAttackCost;
    }
}

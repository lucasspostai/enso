using Enso.Characters.Player;

namespace Enso
{
    [System.Serializable]
    public class PlayerData
    {
        public int Stage;
        
        public int Health;
        public int HealingCharges;
        
        public float Balance;
        
        public bool StrongAttackUnlocked;
        public bool SpecialAttackUnlocked;

        public PlayerData(Player player)
        {
            //Stage

            Health = player.GetHealthSystem().GetMaxHealth();
            HealingCharges = player.HealController.GetMaxHealingCharges();
            Balance = player.GetBalanceSystem().GetMaxBalance();
            StrongAttackUnlocked = player.AttackController.StrongAttackUnlocked;
            SpecialAttackUnlocked = player.AttackController.SpecialAttackUnlocked;
        }
    }
}

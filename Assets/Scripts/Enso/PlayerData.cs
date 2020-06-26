using Enso.Characters.Player;
using Framework.Utils;

namespace Enso
{
    [System.Serializable]
    public class PlayerData
    {
        public int LevelIndex;
        
        public int Health;
        public int HealingCharges;
        public int XpAmount;
        public int Perks;
        
        public float Balance;
        
        public bool StrongAttackUnlocked;
        public bool SpecialAttackUnlocked;

        public PlayerData(Player player)
        {
            LevelIndex = LevelLoader.Instance.CurrentLevelIndex;

            Health = player.GetHealthSystem().GetMaxHealth();
            HealingCharges = player.HealController.GetMaxHealingCharges();
            
            Balance = player.GetBalanceSystem().GetMaxBalance();
            
            StrongAttackUnlocked = player.AttackController.StrongAttackUnlocked;
            SpecialAttackUnlocked = player.AttackController.SpecialAttackUnlocked;
            
            XpAmount = ExperienceManager.Instance.XpAmount;
            Perks = ExperienceManager.Instance.PerksAvailable;
        }
    }
}

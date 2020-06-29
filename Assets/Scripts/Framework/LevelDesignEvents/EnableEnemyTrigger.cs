using Enso.Characters.Enemies;
using Enso.Characters.Player;
using UnityEngine;

namespace Framework.LevelDesignEvents
{
    public class EnableEnemyTrigger : LevelDesignEvent
    {
        private Player player;
        
        [SerializeField] private Enemy[] Enemies;

        private void Start()
        {
            player = FindObjectOfType<Player>();
        }

        public override void Execute()
        {
            base.Execute();

            foreach (var enemy in Enemies)
            {
                if (!enemy.GetHealthSystem().IsDead && !enemy.IsInCombat)
                {
                    enemy.EnterCombatWith(player);
                    enemy.IsInCombat = true;
                    player.EnterCombatWith(enemy);
                }
            }
        }
    }
}

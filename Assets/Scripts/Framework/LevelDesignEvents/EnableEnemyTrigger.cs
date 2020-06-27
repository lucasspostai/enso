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
                enemy.EnterCombatWith(player);
            }
        }
    }
}

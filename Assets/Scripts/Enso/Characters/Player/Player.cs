using Enso.CombatSystem;
using Framework;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class Player : Fighter
    {
        [Header("References")] 
        public PlayerAttackController AttackController;
        public PlayerGuardController GuardController;
        public PlayerRollController RollController;

        public PlayerProperties GetProperties()
        {
            return BaseProperties as PlayerProperties;
        }
    }
}

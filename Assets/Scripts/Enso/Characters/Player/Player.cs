using Enso.CombatSystem;
using Framework;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class Player : Fighter
    {
        [Header("References")] 
        public PlayerMovement Movement;
        public PlayerAttackController AttackController;
        public PlayerGuardController GuardController;
        public CharacterCollisions Collisions;
        public PlayerDodgeRoll DodgeRoll;

        public PlayerProperties GetProperties()
        {
            return BaseProperties as PlayerProperties;
        }
    }
}

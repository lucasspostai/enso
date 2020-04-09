using Framework;
using UnityEngine;

namespace Enso.Characters.Player
{
    public class Player : Fighter
    {
        [Header("References")] 
        public Animator Animator;
        public PlayerMovement Movement;
        public PlayerAttack Attack;
        public PlayerDefense Defense;
        public CharacterCollisions Collisions;
        public PlayerDodgeRoll DodgeRoll;
        public HealthSystem Health;

        public PlayerProperties GetProperties()
        {
            return BaseProperties as PlayerProperties;
        }
    }
}

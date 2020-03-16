using UnityEngine;

namespace Gameplay
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
        public PlayerProperties Properties;
    }
}

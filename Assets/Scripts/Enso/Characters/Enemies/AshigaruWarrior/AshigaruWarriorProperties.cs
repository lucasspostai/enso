using UnityEngine;

namespace Enso.Characters.Enemies.AshigaruWarrior
{
    [CreateAssetMenu(fileName = "AshigaruWarriorProperties", menuName = "Enso/Enemy/Ashigaru Warrior")]
    public class AshigaruWarriorProperties : EnemyProperties
    {
        [Header("AI Distances")] 
        public float GuardDistance = 4f;
        public float AttackDistance = 2f;
    }
}

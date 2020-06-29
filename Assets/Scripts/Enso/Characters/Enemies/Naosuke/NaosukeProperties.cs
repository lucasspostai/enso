using UnityEngine;

namespace Enso.Characters.Enemies.Naosuke
{
    [CreateAssetMenu(fileName = "NaosukeProperties", menuName = "Enso/Enemy/Naosuke")]
    public class NaosukeProperties : EnemyProperties
    {
        [Header("AI Distances")] 
        public float WaitDistance = 6f;
        public float GuardDistance = 6f;
        public float LightAttackDistance = 2.5f;
        public float StrongAttackDistance = 2.5f;
        public float SpecialAttackDistance = 2.5f;
        public float RollDistance = 2.5f;
    }
}

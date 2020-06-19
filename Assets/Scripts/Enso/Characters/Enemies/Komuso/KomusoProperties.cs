using UnityEngine;

namespace Enso.Characters.Enemies.Komuso
{
    [CreateAssetMenu(fileName = "KomusoProperties", menuName = "Enso/Enemy/Komuso")]
    public class KomusoProperties : EnemyProperties
    {
        [Header("AI Distances")] 
        public float GuardDistance = 6f;
        public float LightAttackDistance = 2f;
        public float StrongAttackDistance = 2.5f;
    }
}

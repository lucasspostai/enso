using UnityEngine;

namespace Enso.Characters.Enemies.AshigaruArcher
{
    [CreateAssetMenu(fileName = "AshigaruArcherProperties", menuName = "Enso/Enemy/Ashigaru Archer")]
    public class AshigaruArcherProperties : EnemyProperties
    {
        [Header("AI Distances")] 
        public float ShootArrowDistance = 7f;
        public float RollDistance = 2f;
    }
}
